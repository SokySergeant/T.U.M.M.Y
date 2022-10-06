using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeScript : MonoBehaviour
{

public float spikeSpeed = 3f;

public Rigidbody2D spikebody;

public Transform spikeTransform;

public string whichWayUp;

bool facingLeft;





bool spikeGrounded;
public Transform spikeGroundCheck;
float spikeGroundRadius = 0.1f;
public LayerMask whatIsGround;

bool spikeWall;
public Transform spikeWallCheck;
public float spikeWallRadius = 0.07f;






    // Start is called before the first frame update
    void Start()
    {
        switch(whichWayUp){
            case "up":
                spikeTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                break;

            case "down":
                spikeTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                break;

            case "left":
                spikeTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                break;

            case "right":
                spikeTransform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                break;

            default:
                spikeTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        spikeGrounded = Physics2D.OverlapCircle(spikeGroundCheck.position, spikeGroundRadius, whatIsGround);
        
        if(Physics2D.OverlapCircle(spikeWallCheck.position, spikeWallRadius, whatIsGround)){

            Collider2D[] tempColliders = Physics2D.OverlapCircleAll(spikeWallCheck.position, spikeWallRadius, whatIsGround);

            foreach(Collider2D tempCollider in tempColliders){
                if(tempCollider.transform.root != spikeTransform.root){
                    spikeWall = true;
                    break;
                }else{
                    spikeWall = false;
                }
            }


        }


    }



    void FixedUpdate()
    {
        if(spikeWall || spikeGrounded){
            Flip();
        }

        spikeMove();
    }














    void spikeMove(){
        switch(whichWayUp){
            case "up":
                spikebody.AddForce(Vector2.down * 9.81f);
                if(facingLeft){
                    spikebody.velocity = new Vector2(-spikeSpeed, spikebody.velocity.y);
                }else{
                    spikebody.velocity = new Vector2(spikeSpeed, spikebody.velocity.y);
                }
                break;

            case "down":
                spikebody.AddForce(Vector2.up * 9.81f);
                if(facingLeft){
                    spikebody.velocity = new Vector2(spikeSpeed, spikebody.velocity.y);
                }else{
                    spikebody.velocity = new Vector2(-spikeSpeed, spikebody.velocity.y);
                }
                break;

            case "left":
                spikebody.AddForce(Vector2.right * 9.81f);
                if(facingLeft){
                    spikebody.velocity = new Vector2(spikebody.velocity.x, -spikeSpeed);
                }else{
                    spikebody.velocity = new Vector2(spikebody.velocity.x, spikeSpeed);
                }
                break;

            case "right":
                spikebody.AddForce(Vector2.left * 9.81f);
                if(facingLeft){
                    spikebody.velocity = new Vector2(spikebody.velocity.x, spikeSpeed);
                }else{
                    spikebody.velocity = new Vector2(spikebody.velocity.x, -spikeSpeed);
                }
                break;

            default:
                spikebody.AddForce(Vector2.down * 9.81f);
                if(facingLeft){
                    spikebody.velocity = new Vector2(-spikeSpeed, spikebody.velocity.y);
                }else{
                    spikebody.velocity = new Vector2(spikeSpeed, spikebody.velocity.y);
                }
                break;
        }
    }



    void Flip(){
        facingLeft = !facingLeft;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }







}//CLASS END
