using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class managerScript : MonoBehaviour
{

public GameObject fallingPlatform1WObj;
public Vector2[] fallingPlatform1WLocs;

public GameObject fallingPlatform2WObj;
public Vector2[] fallingPlatform2WLocs;

public GameObject fallingPlatform3WObj;
public Vector2[] fallingPlatform3WLocs;

public GameObject fallingPlatform2HObj;
public Vector2[] fallingPlatform2HLocs;

public GameObject fallingPlatform3HObj;
public Vector2[] fallingPlatform3HLocs;

public GameObject walkEnemyObj;
public Vector2[] walkEnemyLocs;
public string[] walkEnemyWaysUp;

public GameObject flyEnemyObj;
public Vector2[] flyEnemyLocs;
public string[] flyEnemyDir;

public GameObject acidBubbleEnemyObj;
public Vector2[] acidBubbleEnemyLocs;
public float[] acidBubbleEnemyStartWait;
public float[] acidBubbleEnemyWait;
public float[] acidBubbleEnemyJumpForce;

public GameObject movingPlatform1WObj;
public Vector2[] movingPlatform1WLocs;
public float[] movingPlatform1WSpeed;
public string[] movingPlatform1WDir;
public float[] movingPlatform1WWaitTime;

public GameObject movingPlatform2WObj;
public Vector2[] movingPlatform2WLocs;
public float[] movingPlatform2WSpeed;
public string[] movingPlatform2WDir;
public float[] movingPlatform2WWaitTime;

public GameObject movingPlatform3WObj;
public Vector2[] movingPlatform3WLocs;
public float[] movingPlatform3WSpeed;
public string[] movingPlatform3WDir;
public float[] movingPlatform3WWaitTime;

public GameObject movingPlatform2HObj;
public Vector2[] movingPlatform2HLocs;
public float[] movingPlatform2HSpeed;
public string[] movingPlatform2HDir;
public float[] movingPlatform2HWaitTime;

public GameObject movingPlatform3HObj;
public Vector2[] movingPlatform3HLocs;
public float[] movingPlatform3HSpeed;
public string[] movingPlatform3HDir;
public float[] movingPlatform3HWaitTime;





List<GameObject> allObj = new List<GameObject>();

public GameObject self;
public float managerHeight;
public float managerWidth;

public LayerMask whatIsPlayer;

bool playerInside = false;
bool ableToStart = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    //SETUP
        if(Physics2D.OverlapBox(self.transform.position, new Vector2(managerWidth, managerHeight), 0f, whatIsPlayer)){
            playerInside = true;
        }else{
            playerInside = false;
            ableToStart = true;
        }









    //SPAWN
        if(playerInside){
            if(ableToStart){
                foreach(Vector2 fallingPlatform1WLoc in fallingPlatform1WLocs){
                    allObj.Add(Instantiate(fallingPlatform1WObj, fallingPlatform1WLoc, Quaternion.Euler(0, 0, 0)));
                }
            
                foreach(Vector2 fallingPlatform2WLoc in fallingPlatform2WLocs){
                    allObj.Add(Instantiate(fallingPlatform2WObj, fallingPlatform2WLoc, Quaternion.Euler(0, 0, 0)));
                }

                foreach(Vector2 fallingPlatform3WLoc in fallingPlatform3WLocs){
                    allObj.Add(Instantiate(fallingPlatform3WObj, fallingPlatform3WLoc, Quaternion.Euler(0, 0, 0)));
                }

                foreach(Vector2 fallingPlatform2HLoc in fallingPlatform2HLocs){
                    allObj.Add(Instantiate(fallingPlatform2HObj, fallingPlatform2HLoc, Quaternion.Euler(0, 0, 0)));
                }

                foreach(Vector2 fallingPlatform3HLoc in fallingPlatform3HLocs){
                    allObj.Add(Instantiate(fallingPlatform3HObj, fallingPlatform3HLoc, Quaternion.Euler(0, 0, 0)));
                }

                for (int i = 0; i < walkEnemyLocs.Length; i++){
                    GameObject tempObj = Instantiate(walkEnemyObj, walkEnemyLocs[i], Quaternion.Euler(0, 0, 0));
                    tempObj.GetComponent<spikeScript>().whichWayUp = walkEnemyWaysUp[i];
                    allObj.Add(tempObj);
                }

                for (int i = 0; i < flyEnemyLocs.Length; i++){
                    GameObject tempObj = Instantiate(flyEnemyObj, flyEnemyLocs[i], Quaternion.Euler(0, 0, 0));
                    tempObj.GetComponent<flyScript>().startFlyDirection = flyEnemyDir[i];
                    allObj.Add(tempObj);
                }

                for (int i = 0; i < acidBubbleEnemyLocs.Length; i++){
                    GameObject tempObj = Instantiate(acidBubbleEnemyObj, acidBubbleEnemyLocs[i], Quaternion.Euler(0, 0, 0));
                    tempObj.GetComponent<acidBubbleEnemyScript>().secondsToWaitOnStart = acidBubbleEnemyStartWait[i];
                    tempObj.GetComponent<acidBubbleEnemyScript>().secondsToWait = acidBubbleEnemyWait[i];
                    tempObj.GetComponent<acidBubbleEnemyScript>().jumpForce = acidBubbleEnemyJumpForce[i];
                    allObj.Add(tempObj);
                }

                for (int i = 0; i < movingPlatform1WLocs.Length; i++){
                    GameObject tempObj = Instantiate(movingPlatform1WObj, movingPlatform1WLocs[i], Quaternion.Euler(0, 0, 0));
                    tempObj.GetComponent<flyScript>().flySpeed = movingPlatform1WSpeed[i];
                    tempObj.GetComponent<flyScript>().startFlyDirection = movingPlatform1WDir[i];
                    tempObj.GetComponent<flyScript>().secondsToWaitOnHit = movingPlatform1WWaitTime[i];
                    allObj.Add(tempObj);
                }

                for (int i = 0; i < movingPlatform2WLocs.Length; i++){
                    GameObject tempObj = Instantiate(movingPlatform2WObj, movingPlatform2WLocs[i], Quaternion.Euler(0, 0, 0));
                    tempObj.GetComponent<flyScript>().flySpeed = movingPlatform2WSpeed[i];
                    tempObj.GetComponent<flyScript>().startFlyDirection = movingPlatform2WDir[i];
                    tempObj.GetComponent<flyScript>().secondsToWaitOnHit = movingPlatform2WWaitTime[i];
                    allObj.Add(tempObj);
                }
                
                for (int i = 0; i < movingPlatform3WLocs.Length; i++){
                    GameObject tempObj = Instantiate(movingPlatform3WObj, movingPlatform3WLocs[i], Quaternion.Euler(0, 0, 0));
                    tempObj.GetComponent<flyScript>().flySpeed = movingPlatform3WSpeed[i];
                    tempObj.GetComponent<flyScript>().startFlyDirection = movingPlatform3WDir[i];
                    tempObj.GetComponent<flyScript>().secondsToWaitOnHit = movingPlatform3WWaitTime[i];
                    allObj.Add(tempObj);
                }

                for (int i = 0; i < movingPlatform2HLocs.Length; i++){
                    GameObject tempObj = Instantiate(movingPlatform2HObj, movingPlatform2HLocs[i], Quaternion.Euler(0, 0, 0));
                    tempObj.GetComponent<flyScript>().flySpeed = movingPlatform2HSpeed[i];
                    tempObj.GetComponent<flyScript>().startFlyDirection = movingPlatform2HDir[i];
                    tempObj.GetComponent<flyScript>().secondsToWaitOnHit = movingPlatform2HWaitTime[i];
                    allObj.Add(tempObj);
                }

                for (int i = 0; i < movingPlatform3HLocs.Length; i++){
                    GameObject tempObj = Instantiate(movingPlatform3HObj, movingPlatform3HLocs[i], Quaternion.Euler(0, 0, 0));
                    tempObj.GetComponent<flyScript>().flySpeed = movingPlatform3HSpeed[i];
                    tempObj.GetComponent<flyScript>().startFlyDirection = movingPlatform3HDir[i];
                    tempObj.GetComponent<flyScript>().secondsToWaitOnHit = movingPlatform3HWaitTime[i];
                    allObj.Add(tempObj);
                }


















            
            }
            


            ableToStart = false;
        }else{
    //DESPAWN
            foreach(GameObject obj in allObj){
                Destroy(obj);
            }
        }








    }














private void OnDrawGizmos()
{
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(self.transform.position, new Vector2(managerWidth, managerHeight));
}



}//END CLASS