using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] //makes this able to be save in a file

public class savedData
{
    public int hp;
    public bool canWallJump;
    public bool canDoubleJump;
    public bool canDash;
    public float[] position;
    public float[] currentCheckpointPos;
    public float[] currentSaveStationPos;

    public savedData(playerController player){
        hp = player.playerHP;
        canWallJump = player.canWallJump;
        canDoubleJump = player.canDoubleJump;
        canDash = player.canDash;

        position = new float[2];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;

        currentCheckpointPos = new float[2];
        currentCheckpointPos[0] = player.currentCheckpoint.x;
        currentCheckpointPos[1] = player.currentCheckpoint.y;

        currentSaveStationPos = new float[2];
        currentSaveStationPos[0] = player.currentSaveStationPos.x;
        currentSaveStationPos[1] = player.currentSaveStationPos.y;
    }
    
}
