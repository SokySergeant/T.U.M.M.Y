using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingPlatformScript : MonoBehaviour
{

public GameObject selfObj;
public Rigidbody2D selfBody;
public Animator selfAnim;

public Transform groundCheck;

public LayerMask whatIsGround;

public bool fallBool = false;
bool dieBool = true;

public float boxSize1 = 0.9f;
public float boxSize2 = 1.1f;

public bool kick = false;

bool canDie = false;

bool canDie2 = true;
bool canRespawn = true;

public float secondsToWaitOnDeath = 3f;

Vector2 spawnPos;

    // Start is called before the first frame update
    void Start()
    {
        selfBody.GetComponent<BoxCollider2D>().enabled = true;
        spawnPos = selfObj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(fallBool){
            StartCoroutine(fall());
        }

        if(canDie && Physics2D.OverlapBox(groundCheck.position, new Vector2(boxSize1, boxSize2), 0f, whatIsGround)){
            Collider2D[] tempCols = Physics2D.OverlapBoxAll(groundCheck.position, new Vector2(boxSize1, boxSize2), 0f, whatIsGround);

            foreach(Collider2D tempCol in tempCols){
                if(tempCol.transform.root != selfBody.transform.root){
                StartCoroutine(die());
                }
            }
        }
    }



IEnumerator fall(){
    if(dieBool){
        dieBool = false;

        selfAnim.SetBool("shake", true);

        yield return new WaitForSeconds(2f);

        selfAnim.SetBool("shake", false);

        selfBody.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        selfBody.AddForce(new Vector2(0, -1)); //Add a lil bit of force to jolt it awake, doesn't start falling without

        canDie = true;


        StartCoroutine(respawn());
    }
    
}



IEnumerator die(){
    if(canDie2){
        canDie2 = false;
        selfAnim.SetBool("die", true);

        kick = true;

        yield return new WaitForSeconds(0.3f);

        selfBody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        selfBody.GetComponent<BoxCollider2D>().enabled = false;

        selfAnim.SetBool("die", false);
    }
}


IEnumerator respawn(){
    if(canRespawn){
        canRespawn = false;

        

        yield return new WaitForSeconds(secondsToWaitOnDeath);

        selfBody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

        selfAnim.SetBool("respawn", true);
        selfObj.transform.position = spawnPos;
        
        selfBody.GetComponent<BoxCollider2D>().enabled = true;
        dieBool = true;
        canDie = false;
        kick = false;
        fallBool = false;
        canDie2 = true;


        

        yield return new WaitForSeconds(0.1f);

        selfAnim.SetBool("respawn", false);

        canRespawn = true;
    }
    
}













private void OnDrawGizmos()
{
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(groundCheck.position, new Vector2(boxSize1, boxSize2));
}

}
