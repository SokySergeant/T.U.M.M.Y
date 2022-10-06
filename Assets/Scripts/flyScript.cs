using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyScript : MonoBehaviour
{

public Transform flySelf;
public float flySpeed = 3f;

public float flyRadius = 0.49f;
public float flyRadius2 = 0.49f;

float flyX = 1f;
float flyY = 0f;

public LayerMask whatIsUp;
public LayerMask whatIsDown;
public LayerMask whatIsLeft;
public LayerMask whatIsRight;

public string startFlyDirection;

public bool isSquare;

public float secondsToWaitOnHit = 0f;
bool canMove = true;
bool canWait = true; //Makes it wait until the entity has gotten a bit away from the blocker, to not trigger a billion times




    // Start is called before the first frame update
    void Start()
    {
        switch(startFlyDirection){
            case "up":
                flyX = 0f;
                flyY = 1f;
                break;

            case "down":
                flyX = 0f;
                flyY = -1f;
                break;

            case "left":
                flyX = -1f;
                flyY = 0f;
                break;

            case "right":
                flyX = 1f;
                flyY = 0f;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {





    }

    void FixedUpdate()
    {
        if(isSquare){
            checkDirectionsSquare();
        }else{
            checkDirections();
        }
        
        flyMove();
    }






















void checkDirections(){
    if(Physics2D.OverlapCircle(flySelf.position, flyRadius, whatIsUp)){
        flyX = 0f;
        flyY = 1f;
        StartCoroutine(waitTimer());
    }else if(Physics2D.OverlapCircle(flySelf.position, flyRadius, whatIsDown)){
        flyX = 0f;
        flyY = -1f;
        StartCoroutine(waitTimer());
    }else if(Physics2D.OverlapCircle(flySelf.position, flyRadius, whatIsLeft)){
        flyX = -1f;
        flyY = 0f;
        StartCoroutine(waitTimer());
    }else if(Physics2D.OverlapCircle(flySelf.position, flyRadius, whatIsRight)){
        flyX = 1f;
        flyY = 0f;
        StartCoroutine(waitTimer());
    }
}


void checkDirectionsSquare(){
    if(Physics2D.OverlapBox(flySelf.position, new Vector2(flyRadius, flyRadius2), 0, whatIsUp)){
        flyX = 0f;
        flyY = 1f;
        StartCoroutine(waitTimer());
    }else if(Physics2D.OverlapBox(flySelf.position, new Vector2(flyRadius, flyRadius2), 0, whatIsDown)){
        flyX = 0f;
        flyY = -1f;
        StartCoroutine(waitTimer());
    }else if(Physics2D.OverlapBox(flySelf.position, new Vector2(flyRadius, flyRadius2), 0, whatIsLeft)){
        flyX = -1f;
        flyY = 0f;
        StartCoroutine(waitTimer());
    }else if(Physics2D.OverlapBox(flySelf.position, new Vector2(flyRadius, flyRadius2), 0, whatIsRight)){
        flyX = 1f;
        flyY = 0f;
        StartCoroutine(waitTimer());
    }
}






void flyMove(){
    if(canMove){
        flySelf.position += new Vector3(flyX * Time.fixedDeltaTime * flySpeed, flyY * Time.fixedDeltaTime * flySpeed, 0);
    }
}


IEnumerator waitTimer(){
    if(canWait){
        canMove = false;
        canWait = false;

        yield return new WaitForSeconds(secondsToWaitOnHit);
        canMove = true;

        yield return new WaitForSeconds(1f);
        canWait = true;
    }
}















/*
private void OnDrawGizmos()
{
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(flySelf.position, flyRadius);
}
*/


private void OnDrawGizmos()
{
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(flySelf.position, new Vector2(flyRadius, flyRadius2));
}


}//END CLASS