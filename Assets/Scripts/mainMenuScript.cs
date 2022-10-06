using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenuScript : MonoBehaviour
{

    public static bool isNewGame;
    
    public void newGame(){
        isNewGame = true;
        SceneManager.LoadScene("MainGameScene");
    }

    public void continueGame(){
        if(saveDataScript.loadGame() != null){ //only lets you click continue if theres an existing save file
            isNewGame = false;
            SceneManager.LoadScene("MainGameScene");
        }
    }

    public void quitGame(){
        Application.Quit();
    }


}
