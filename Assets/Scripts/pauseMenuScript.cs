using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseMenuScript : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuObj;
    public playerController player;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(isPaused){
                resume();
            }else{
                pause();
            }
        }
    }




    public void resume(){
        pauseMenuObj.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void pause(){
        pauseMenuObj.SetActive(true);
        Time.timeScale = 0f; //freezes game
        isPaused = true;
    }

    public void menuBtn(){
        saveDataScript.saveGame(player);
        SceneManager.LoadScene("MainMenuScene");
    }

    public void exitBtn(){
        saveDataScript.saveGame(player);
        Application.Quit();
    }


}
