using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeBossScript : MonoBehaviour
{

public bool isActive = false;

public GameObject self;
public Animator selfAnim;
public Rigidbody2D selfBody;
public Transform wallCheck;
public LayerMask whatIsGround;
public float wallCheckRadius = 0.1f;

Vector2 startPoint = new Vector2(89.5f, 43.5f);

bool canStart = true;

public float selfSpeed = 5f;
float selfJumpForce = 500f;

bool facingLeft = false;
bool inAction = false;
bool doChargeBool = false;
bool doWalkBool = false;

bool hitWall = false;
bool isVulnerable = false;
bool canDoHit = false;

float selfRadius = 1.5f;
public LayerMask whatIsSpike;

public float startRadius = 8.5f;
public LayerMask whatIsPlayer;

bool isDead = false;

//MOVING PLATFORM STUFF
public GameObject mp1W;
Vector2[] mp1WLocs = {};
string[] mp1WDirs = {};

public GameObject mp3H;
Vector2[] mp3HLocs = {};
string[] mp3HDirs = {};

List<GameObject> allmps = new List<GameObject>();


public GameObject fadeoutObj;


public int actionCheck = 0;
/*
ACTION CHART:
0: jump
1: charge
*/

    // Start is called before the first frame update
    void Start()
    {
        selfBody.position = startPoint;
    }//END START

    // Update is called once per frame
    void Update()
    {
        

    }//END UPDATE





    void FixedUpdate()
    {
        //START
        if(Physics2D.OverlapCircle(selfBody.position, startRadius, whatIsPlayer) && canStart){
            canStart = false;
            StartCoroutine(start());
        }

        if(Physics2D.OverlapBox(selfBody.position, new Vector2(selfRadius * 2.1f, selfRadius * 2), 0, whatIsPlayer)){
            Collider2D tempPlayer = Physics2D.OverlapBox(selfBody.position, new Vector2(selfRadius * 2.1f, selfRadius * 2), 0, whatIsPlayer);
            if(tempPlayer.GetComponent<playerController>().playerHP == 0){
                StartCoroutine(reset());
            }
        }

        //FLIP
        if(Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, whatIsGround)){
            Flip();
            hitWall = true;
        }else{
            hitWall = false;
        }


        if(doChargeBool){
            StartCoroutine(doCharge());
        }


        if(isActive){
            StartCoroutine(doActions());
        }


        if(doWalkBool){
            doWalk();
        }


        if(isVulnerable && canDoHit && Physics2D.OverlapCircle(selfBody.position, selfRadius, whatIsSpike)){
            StartCoroutine(getHit());
        }

        

        
    }//END FIXEDUPDATE



    IEnumerator doActions(){
        if(!inAction && !isDead){
            inAction = true;
            
            doWalkBool = true;
            selfAnim.SetTrigger("walk");

            int randomTime = Random.Range(1, 4); //get time to wait between next action (Random.Range(min, max) max exclusive)
            int randomAction = Random.Range(0, 2); //get next action    

            yield return new WaitForSeconds(randomTime);

            actionCheck = randomAction;

            doWalkBool = false;

            switch(actionCheck){
                case 0:
                StartCoroutine(doJump());
                break;

                case 1:
                StartCoroutine(doShake());
                break;

                default:
                break;
            }
        }
    }



    void doWalk(){
        if(facingLeft){
            selfBody.velocity = new Vector2(-selfSpeed, selfBody.velocity.y);
        }else{
            selfBody.velocity = new Vector2(selfSpeed, selfBody.velocity.y);
        }
    }



    IEnumerator doJump(){
        selfAnim.SetTrigger("idle");

        selfBody.AddForce(new Vector2(0, selfJumpForce));

        yield return new WaitForSeconds(1.5f);

        inAction = false;
    }



    IEnumerator doShake(){
        selfAnim.SetTrigger("shake");
        yield return new WaitForSeconds(2f);
        selfAnim.SetTrigger("idle");

        doChargeBool = true;
    }


int amountOfCharges = 0;
    IEnumerator doCharge(){
        if(facingLeft){
                selfBody.velocity = new Vector2(-selfSpeed * 3, selfBody.velocity.y);
            }else{
                selfBody.velocity = new Vector2(selfSpeed * 3, selfBody.velocity.y);
            }

        if(hitWall){
            doChargeBool = false;
            selfBody.velocity = new Vector2(0, 0);
            selfAnim.SetTrigger("idle");

            if(facingLeft){
                selfBody.AddForce(new Vector2(-100, 100));
            }else{
                selfBody.AddForce(new Vector2(100, 100));
            }


            //get dazed
            if(amountOfCharges == 4){ //repeats the charge 5 times
                selfAnim.SetTrigger("spikeDown");
                yield return new WaitForSeconds(0.4f);
                selfAnim.SetTrigger("vulnerable");
                isVulnerable = true;
                canDoHit = true;

                StartCoroutine(spawnMovingPlatforms());

                yield return new WaitForSeconds(10f);
                isVulnerable = false;
                
                if(!isDead && isActive){//this exists because hitting him at the very last moment of his daze can break him into an endless loop of this animation
                    selfAnim.SetTrigger("spikeUp");
                }
                yield return new WaitForSeconds(0.4f); //wait for the spikeUp animation to end

                inAction = false;
                amountOfCharges = 0;
            }else{
                doChargeBool = true;
                amountOfCharges++;
            }

            
        }
    }



    IEnumerator getHit(){
        canDoHit = false;

        selfBody.AddForce(new Vector2(0, 1000));

        GetComponent<enemyScript>().currentHealth -= 20;

        if(GetComponent<enemyScript>().currentHealth == 0){ //die
            isDead = true;
            selfAnim.SetTrigger("die");
            yield return new WaitForSeconds(2.5f);
            fadeoutObj.GetComponent<fadeoutScript>().backToMenuFade(3f);
            Object.Destroy(self);
        }
    }





    IEnumerator spawnMovingPlatforms(){
        int randMP = Random.Range(0, 3); //0, 3 default
        
        //resetting the arrays
        mp1WLocs = new Vector2[] {};
        mp1WDirs = new string[] {};
        
        mp3HLocs = new Vector2[] {};
        mp3HDirs = new string[] {};

        switch(randMP){//choose one of 3 different setups of moving platforms
            case 0:
                mp1WLocs = new Vector2[] {new Vector2(112.5f, 48.5f), new Vector2(85.5f, 48.5f), new Vector2(101f, 58.5f), new Vector2(97f, 58.5f)};
                mp1WDirs = new string[] {"left", "right", "down", "down"};
                break;
            
            case 1:
                mp3HLocs = new Vector2[] {new Vector2(112.5f, 49.5f), new Vector2(85.5f, 49.5f), new Vector2(100.5f, 58.5f), new Vector2(97.5f, 58.5f)};
                mp3HDirs = new string[] {"left", "right", "down", "down"};
                break;
            
            case 2:
                mp1WLocs = new Vector2[] {new Vector2(101f, 58.5f), new Vector2(97f, 58.5f), new Vector2(99.0f, 58.5f)};
                mp1WDirs = new string[] {"down", "down", "down"};

                mp3HLocs = new Vector2[] {new Vector2(104.5f, 58.5f), new Vector2(93.5f, 58.5f)};
                mp3HDirs = new string[] {"down", "down"};
                break;

            default:
            break;
        }

        for (int i = 0; i < mp1WLocs.Length; i++){
            GameObject tempMP = Instantiate(mp1W, mp1WLocs[i], Quaternion.Euler(0, 0, 0));
            tempMP.GetComponent<flyScript>().startFlyDirection = mp1WDirs[i];
            tempMP.GetComponent<flyScript>().flySpeed = 10;
            tempMP.GetComponent<flyScript>().secondsToWaitOnHit = 6f;
            allmps.Add(tempMP);
        }

        for (int i = 0; i < mp3HLocs.Length; i++){
            GameObject tempMP = Instantiate(mp3H, mp3HLocs[i], Quaternion.Euler(0, 0, 0));
            tempMP.GetComponent<flyScript>().startFlyDirection = mp3HDirs[i];
            tempMP.GetComponent<flyScript>().flySpeed = 10;
            tempMP.GetComponent<flyScript>().secondsToWaitOnHit = 6f;
            allmps.Add(tempMP);
        }

        yield return new WaitForSeconds(9f);

        foreach(GameObject obj in allmps){//delete all moving platforms after they've flown out of view
            Destroy(obj);
        }
    }






    IEnumerator start(){
        selfAnim.SetTrigger("awake");
        yield return new WaitForSeconds(2f);
        selfAnim.SetTrigger("spikeUp");
        yield return new WaitForSeconds(0.4f);
        isActive = true;
    }





    IEnumerator reset(){
        isActive = false;
        yield return new WaitForSeconds(2f);
        selfBody.position = startPoint;
        GetComponent<enemyScript>().currentHealth = GetComponent<enemyScript>().maxHealth;
        selfAnim.SetTrigger("sleep");
        
        canStart = true;
    }














    void Flip(){
        facingLeft = !facingLeft;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }





    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(selfBody.position, selfRadius);
        Gizmos.DrawWireCube(selfBody.position, new Vector2(selfRadius * 2.1f, selfRadius * 2));
    }



}//END CLASS