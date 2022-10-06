using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorScript : MonoBehaviour
{

public GameObject self;
public string doorName;
public audioManagerScript audioController;



public void unlock(){
    audioController.playSound("unlock");
    Object.Destroy(self);
}


}//END CLASS
