using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upgradeScript : MonoBehaviour
{
string upgradeName;
public audioManagerScript audioController;

public GameObject wallJumpInfo;
public GameObject doubleJumpInfo;
public GameObject dashInfo;

    void Start(){
        upgradeName = GetComponent<itemScript>().itemName;
    }

    public void addUpgrade(playerController player){
        switch(upgradeName)
        {
            case "wallJump":
            player.canWallJump = true;
            audioController.playSound("wallJumpUpgrade");
            wallJumpInfo.GetComponent<fadeoutScript>().showInfoCard();
            break;

            case "doubleJump":
            player.canDoubleJump = true;
            audioController.playSound("doubleJumpUpgrade");
            doubleJumpInfo.GetComponent<fadeoutScript>().showInfoCard();
            break;

            case "dash":
            player.canDash = true;
            audioController.playSound("dashUpgrade");
            dashInfo.GetComponent<fadeoutScript>().showInfoCard();
            break; 

            default:
            break;
        }
    }


    
}
