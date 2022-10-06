using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class acidBubbleEnemyScript : MonoBehaviour
{

    int oldGrav = 1;
    public float secondsToWait = 0f;
    public float jumpForce = 500f;

    bool canJump = true;

    public Rigidbody2D selfBody;
    public Transform self;
    public float selfRadius = 0.49f;
    public LayerMask whatIsUp;

    public float secondsToWaitOnStart = 0f;
    bool canStart = false;

    



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(startWaiter());
    }


    void FixedUpdate()
    {
        if(Physics2D.OverlapCircle(self.position, selfRadius, whatIsUp)){
            StartCoroutine(acidBubbleJump());
        }
    }




IEnumerator acidBubbleJump(){
    if(canJump){
        canJump = false;

        selfBody.velocity = new Vector2(0, 0);
        selfBody.gravityScale = 0;

    if(canStart){
        yield return new WaitForSeconds(secondsToWait);

        selfBody.gravityScale = oldGrav;
        selfBody.AddForce(new Vector2(0, jumpForce));

        yield return new WaitForSeconds(1f);
    }
        canJump = true;
    }
}


IEnumerator startWaiter(){
    yield return new WaitForSeconds(secondsToWaitOnStart);
    canStart = true;
}










}//END CLASS
