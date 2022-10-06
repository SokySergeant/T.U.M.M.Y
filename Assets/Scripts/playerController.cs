using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
//BASIC SETUP
    Vector2 introPos = new Vector2(245.5f, 80.5f);

    [HideInInspector] public bool canWallJump = false; //saved
    [HideInInspector] public bool canDoubleJump = false; //saved 
    [HideInInspector] public bool canDash = false; //saved

    const int maxPlayerHP = 5;
    [HideInInspector] public int playerHP = 5; //saved
    float hMove;
    float jumpForce = 600f;
    float speed = 7f;
    public Transform player;
    public Rigidbody2D bodyboy;
    int oldGrav = 3;
    public SpriteRenderer playerSpriteRenderer;


//HEALTH STUFF SETUP
    public GameObject healthObj;

    bool hasDied = false;

    public GameObject fadeoutObj;


//ITEM SETUP
    List<string> keyList = new List<string>();
    public LayerMask whatIsItem;



//DAMAGE SETUP
    public LayerMask whatIsHazard;
    float playerRadius = 1f;
    bool isTouchingHazard = false;
    int hazardDamage = 1;

    bool takeDamageCooldownDone = true;
    bool hazardCooldownDone = true;


//CONSTANTS SETUP
    float oldSpeed;
    float oldJump;
    float oldDashSpeed;


//CHECKPOINT SETUP
    [HideInInspector] public Vector2 currentCheckpoint;
    public LayerMask whatIsCheckpoint;

    Collider2D currentSaveStation;
    [HideInInspector] public Vector2 currentSaveStationPos;
    public LayerMask whatIsSaveStation;

    Vector2 startPoint = new Vector2(249f, 34.5f);

    bool saveStationSFXBool = true;



//GROUND SETUP
    bool grounded = false;
    public Transform groundCheck;
    float groundRadius = 0.2f;
    public LayerMask whatIsGround;
    public LayerMask whatIsMovingPlatform;


    bool facingLeft;


    public Animator anim;



//JUMP AND DASH SETUP
    int maxJump = 1;
    int maxDash = 0;
    float dashSpeed = 1400f;
    bool dashCooldownDone = true;
    bool dashInProgress = false;

    bool canResetJump = true;
    bool canMove = true;


//WALL SETUP
    bool isTouchingWall = false;
    public Transform frontCheck;
    bool wallSliding;
    float wallSlidingSpeed = 1f;
    float wallSlidingFlingForce = 250f;
    bool flingInProgress = false;


//ATTACK SETUP
/*
    public Transform attackPoint;
    float attackRange = 0.5f;
    bool attackInProgress = false;
    int playerDamage = 20;
*/

    public LayerMask enemyLayer;

    public LayerMask whatIsDoor;

    public audioManagerScript audioController;

    public GameObject basicInfo;




    // Start is called before the first frame update
    void Start()
    {
    oldSpeed = speed;
    oldJump = jumpForce;
    oldDashSpeed = dashSpeed;

    currentSaveStationPos = startPoint; //this will usually get overwritten by the save file

    Time.timeScale = 1f; //resets this incase the person exits and reenters using the pause menu, which sets it to 0f
    pauseMenuScript.isPaused = false; //^

    if(mainMenuScript.isNewGame){
        StartCoroutine(doIntro());
    }else{
        loadGame();
    }


    }//START CLOSE







    // Update is called once per frame
    void Update()
    {
    //MOVEMENT
        hMove = Input.GetAxisRaw("Horizontal") * speed;







    //JUMP
        if(Input.GetButtonDown("Jump") && !flingInProgress && jumpForce != 0 && !pauseMenuScript.isPaused){
            if(wallSliding && canWallJump){
                audioController.playSound("jump");
                bodyboy.velocity = new Vector2(0, 0);
                StartCoroutine(wallFling());
                maxJump = 0;
                maxDash = 0;
            }else if(maxJump > 0 && !wallSliding){
                audioController.playSound("jump");
                maxJump -= 1;
                bodyboy.velocity = new Vector2(0, 0);
                bodyboy.AddForce(new Vector2(0, jumpForce));
            }

            StartCoroutine(resetMaxes());
        }






    //DASH
        if((maxDash > 0) && dashCooldownDone && Input.GetKeyDown(KeyCode.LeftShift)){
            StartCoroutine(dash());
            maxDash -= 1;
        }






/*
    //ATTACK
        if(!dashInProgress && !wallSliding && !attackInProgress && Input.GetKeyDown(KeyCode.F)){
            StartCoroutine(attack());
        }
*/






    //GET ITEM
        if(Physics2D.OverlapCircle(player.position, playerRadius, whatIsItem)){
            getItem();
        }






    
    //INTERACT WITH DOOR
        if(Physics2D.OverlapCircle(frontCheck.position, groundRadius, whatIsDoor)){
            unlockDoor();
        }



    }//UPDATE CLOSE
    











    void FixedUpdate()
    {
    //ANIMATION SETUP
        anim.SetFloat("vSpeed", bodyboy.velocity.y);

        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        anim.SetBool("ground", grounded);

        isTouchingWall = Physics2D.OverlapCircle(frontCheck.position, groundRadius, whatIsGround);

        if(grounded){
            if(canResetJump){
                canResetJump = false;
                if(canDoubleJump){
                    maxJump = 2;
                }else{
                    maxJump = 1;
                }
            }
            

            if(canDash){
                maxDash = 1;
            }
        }




    //MOVING PLATFORM
        if(Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsMovingPlatform) | (Physics2D.OverlapCircle(frontCheck.position, groundRadius, whatIsMovingPlatform) && hMove != 0)){

            Collider2D theMovingPlatform = null;

            if(Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsMovingPlatform)){
                theMovingPlatform = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsMovingPlatform);
            }else if(Physics2D.OverlapCircle(frontCheck.position, groundRadius, whatIsMovingPlatform)){
                theMovingPlatform = Physics2D.OverlapCircle(frontCheck.position, groundRadius, whatIsMovingPlatform);
            }


            bool kick; //to get the player off the falling platform before it deletes itself and its children, thus potentially the player

            if(theMovingPlatform.GetComponent<fallingPlatformScript>() != null){
                theMovingPlatform.GetComponent<fallingPlatformScript>().fallBool = true;
                kick = theMovingPlatform.GetComponent<fallingPlatformScript>().kick;
            }else{
                kick = false;
            }

            if(!kick){
                player.SetParent(theMovingPlatform.transform);
            }else{
                player.SetParent(null);
            }

        }else{
            player.SetParent(null);
        }







    //MOVE
        if(!flingInProgress && !dashInProgress){
            player.position += new Vector3(hMove * Time.fixedDeltaTime, 0, 0);
        }
        
        anim.SetFloat("speed", Mathf.Abs(hMove));
        









    //FLIP
    //if(!attackInProgress){
        if (hMove < 0 && !facingLeft){
            Flip();
        }else if (hMove > 0 && facingLeft){
            Flip();
        }
    //}









    //WALL SLIDING
        if(isTouchingWall && !grounded && hMove != 0){
            wallSliding = true;
        }else{
            wallSliding = false;
        }

        if(wallSliding){
            bodyboy.velocity = new Vector2(bodyboy.velocity.x, Mathf.Clamp(bodyboy.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }

        anim.SetBool("wall", wallSliding);









    //CHECKPOINT
    if(Physics2D.OverlapCircle(player.position, playerRadius, whatIsCheckpoint)){
        currentCheckpoint = Physics2D.OverlapCircle(player.position, playerRadius, whatIsCheckpoint).transform.position;
    }




    //SAVE STATIONS
    if(Physics2D.OverlapCircle(player.position, playerRadius, whatIsSaveStation)){
        if(saveStationSFXBool){
            audioController.playSound("saveStation");
            saveStationSFXBool = false;
        }
        
        currentSaveStation = Physics2D.OverlapCircle(player.position, playerRadius, whatIsSaveStation);
        currentSaveStationPos = currentSaveStation.transform.position;
        playerHP = maxPlayerHP;
        healthObj.GetComponent<healthBarScript>().updateHealth(playerHP);
        StartCoroutine(currentSaveStation.GetComponent<saveStationScript>().lightUp());
    }else{
        saveStationSFXBool = true;
    }








    //HAZARDS
    isTouchingHazard = Physics2D.OverlapBox(player.position, new Vector2(playerRadius * 0.6f, playerRadius * 0.6f), 0, whatIsHazard);

    if(isTouchingHazard){
        StartCoroutine(takeDamage(hazardDamage, true, null));
    }







    //TAKE DAMAGE
    if(Physics2D.OverlapBox(player.position, new Vector2(playerRadius, playerRadius), 0, enemyLayer)){
        Collider2D hittingEnemy = Physics2D.OverlapBox(player.position, new Vector2(playerRadius, playerRadius), 0, enemyLayer);

        int enemyDamage = hittingEnemy.GetComponent<enemyScript>().enemyDamage;
        Transform enemyTransform = hittingEnemy.GetComponent<Transform>();

        StartCoroutine(takeDamage(enemyDamage, false, enemyTransform));
    }










    }//FIXEDUPDATE CLOSE


















//FLIP FUNCTION
    void Flip(){
        facingLeft = !facingLeft;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }





//RESETS JUMP AND DASH MAX
    IEnumerator resetMaxes(){
        yield return new WaitForSeconds(0.1f);
        canResetJump = true;
    }







//DASH ENUMERATOR
    IEnumerator dash(){
        audioController.playSound("dash");
        
        dashCooldownDone = false;
        dashInProgress = true;
        anim.SetBool("dashing", dashInProgress);

        bodyboy.velocity = new Vector2(bodyboy.velocity.x, 0); //resets only vertical velocity


        bodyboy.gravityScale = 0; //and then turning off gravity

        if(facingLeft){
            bodyboy.AddForce(new Vector2(-dashSpeed, 0));
        }else{
            bodyboy.AddForce(new Vector2(dashSpeed, 0));
        }

        yield return new WaitForSeconds(0.15f);
        bodyboy.velocity = new Vector2(0, 0);

        if(canMove){
            bodyboy.gravityScale = oldGrav;
        }

        dashInProgress = false;
        anim.SetBool("dashing", dashInProgress);

    //COOLDOWN
        yield return new WaitForSeconds(0.3f);
        dashCooldownDone = true;
}











//WALL SLIDING FLING ENUMERATOR
    IEnumerator wallFling(){
        flingInProgress = true;
        if(facingLeft){
                bodyboy.AddForce(new Vector2(wallSlidingFlingForce, jumpForce));
            }else{
                bodyboy.AddForce(new Vector2(-wallSlidingFlingForce, jumpForce));
            }

        yield return new WaitForSeconds(0.5f);

        bodyboy.velocity = new Vector2(0, bodyboy.velocity.y);

        flingInProgress = false;
    }









/*

//ATTACK ENUMERATOR
    IEnumerator attack(){
        attackInProgress = true;
        anim.SetBool("attacking", true);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach(Collider2D enemy in hitEnemies){
            enemy.GetComponent<enemyScript>().takeDamage(playerDamage);
        }


        yield return new WaitForSeconds(0.05f);
        anim.SetBool("attacking", false);



        yield return new WaitForSeconds(0.4f);
        attackInProgress = false;
    }

*/









//TAKE DAMAGE ENUMERATOR
    IEnumerator takeDamage(int damage, bool hazard, Transform enemyTransform){
        if(takeDamageCooldownDone && hazardCooldownDone){
            audioController.playSound("hurt");

            takeDamageCooldownDone = false;

            playerHP -= damage;
            healthObj.GetComponent<healthBarScript>().updateHealth(playerHP);

            if(playerHP <= 0){//die at 0 hp
                hasDied = true;
                StartCoroutine(die());
                yield break;//this only breaks out of the first if statement
            }else{
                hasDied = false;
            }


            if(!hazard){
                int hurtFlingX;
                if(enemyTransform.position.x > player.position.x){
                    hurtFlingX = -500;
                }else{
                    hurtFlingX = 500;
                }

                bodyboy.velocity = new Vector2(0, 0);
                bodyboy.AddForce(new Vector2(hurtFlingX, 500));

                speed = 0f;
                yield return new WaitForSeconds(0.15f);
                speed = oldSpeed;

                bodyboy.velocity = new Vector2(bodyboy.velocity.x / 5, bodyboy.velocity.y / 5);

                for (int i = 0; i < 5; i++){
                    playerSpriteRenderer.color = new Color(1f, 0.5f, 0.5f, 1f);
                    yield return new WaitForSeconds(0.1f);
                    playerSpriteRenderer.color = new Color(1, 1, 1, 1);
                    yield return new WaitForSeconds(0.1f);
                }
            }else{
                stopMovement();

                for (int i = 0; i < 5; i++){
                    playerSpriteRenderer.color = new Color(1f, 0.5f, 0.5f, 1f);
                    yield return new WaitForSeconds(0.1f);
                    playerSpriteRenderer.color = new Color(1, 1, 1, 1);
                    yield return new WaitForSeconds(0.1f);
                }

                respawn();

                yield return new WaitForSeconds(0.5f);

                startMovement();
            }

            takeDamageCooldownDone = true;

        }else if(hazard && hazardCooldownDone && !hasDied){ //using hasDied to not run this and respawn the player at a checkpoint rather than a save station upon death
            hazardCooldownDone = false;

            stopMovement();

            for (int i = 0; i < 5; i++){
                playerSpriteRenderer.color = new Color(1f, 0.5f, 0.5f, 1f);
                yield return new WaitForSeconds(0.1f);
                playerSpriteRenderer.color = new Color(1, 1, 1, 1);
                yield return new WaitForSeconds(0.1f);
            }

            respawn();

            yield return new WaitForSeconds(0.5f);

            startMovement();

            hazardCooldownDone = true;
        }
    }







//STOP MOVEMENT FUNCTION
    void stopMovement(){
        canMove = false;
        speed = 0;
        bodyboy.velocity = new Vector2(0, 0);
        jumpForce = 0;
        bodyboy.gravityScale = 0;
        dashSpeed = 0;
    }

//START MOVEMENT FUNCTION
    void startMovement(){
        canMove = true;
        speed = oldSpeed;
        bodyboy.gravityScale = oldGrav;
        jumpForce = oldJump;
        dashSpeed = oldDashSpeed;
    }














//RESPAWN TO LATEST CHECKPOINT FUNCTION
    void respawn(){
        player.position = currentCheckpoint;
    }








//RESPAWN TO LATEST SAVE STATION
    IEnumerator die(){

        stopMovement();
        yield return new WaitForSeconds(1f);
        StartCoroutine(fadeoutObj.GetComponent<fadeoutScript>().fade(true));
        yield return new WaitForSeconds(1f);

        //teleport to last save station
        player.position = currentSaveStationPos;


        bodyboy.velocity = new Vector2(0, 0);

        playerHP = maxPlayerHP;
        healthObj.GetComponent<healthBarScript>().updateHealth(playerHP);
        hasDied = false;
        takeDamageCooldownDone = true;//do this here because code doesn't reach down there after breaking earlier

        StartCoroutine(fadeoutObj.GetComponent<fadeoutScript>().fade(false));
        yield return new WaitForSeconds(1f);

        startMovement();
    }










//GET ITEM FUNCTION
    void getItem(){
        Collider2D gottenItem = Physics2D.OverlapCircle(player.position, playerRadius, whatIsItem);

        string tempItemType = gottenItem.GetComponent<itemScript>().itemType;
        string tempItemName = gottenItem.GetComponent<itemScript>().itemName;

        switch(tempItemType){
            case "key":
                audioController.playSound("key");
                keyList.Add(tempItemName);
                break;

            case "upgrade":
                gottenItem.GetComponent<upgradeScript>().addUpgrade(this);
                break;

            default:
                break;
        }

        gottenItem.GetComponent<itemScript>().removeItem(); //deletes the item from the world

    }












//UNLOCK DOOR FUNCTION
    void unlockDoor(){
        Collider2D theDoor = Physics2D.OverlapCircle(frontCheck.position, groundRadius, whatIsDoor);
        string doorName = theDoor.GetComponent<doorScript>().doorName;

        foreach(string i in keyList){
            if(i == doorName){
                theDoor.GetComponent<doorScript>().unlock();
            }
        }
        
    }






//SAVE GAME
    public void saveGame(){
        saveDataScript.saveGame(this); //function to save the game
    }


//LOAD GAME
    public void loadGame(){
        savedData data = saveDataScript.loadGame(); //get saved data

        playerHP = data.hp;
        canWallJump = data.canWallJump;
        canDoubleJump = data.canDoubleJump;
        canDash = data.canDash;

        Vector2 tempPos;
        tempPos.x = data.position[0];
        tempPos.y = data.position[1];
        player.position = tempPos;

        Vector2 tempCheckpointPos;
        tempCheckpointPos.x = data.currentCheckpointPos[0];
        tempCheckpointPos.y = data.currentCheckpointPos[1];
        currentCheckpoint = tempCheckpointPos;

        Vector2 tempSaveStationPos;
        tempSaveStationPos.x = data.currentSaveStationPos[0];
        tempSaveStationPos.y = data.currentSaveStationPos[1];
        currentSaveStationPos = tempSaveStationPos;

        healthObj.GetComponent<healthBarScript>().updateHealth(playerHP);
    }




    IEnumerator doIntro(){
        stopMovement();
        bodyboy.gravityScale = oldGrav; //stopMovement() turns off gravity, turn it back on here
        player.position = introPos;
        yield return new WaitForSeconds(2f);
        bodyboy.AddForce(new Vector2(200f, oldJump)); //using oldJump because stopMovement() sets jumpForce to 0
        yield return new WaitForSeconds(3f);
        startMovement();
        basicInfo.GetComponent<fadeoutScript>().showInfoCard();
    }










//GIZMOS
private void OnDrawGizmos()
{
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(player.position, new Vector2(playerRadius, playerRadius));
}






}//CLASS END





