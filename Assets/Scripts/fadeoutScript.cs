using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class fadeoutScript : MonoBehaviour
{

public Image selfImg;



public IEnumerator fade(bool fadeout, int fadeSpeed = 5){
    if(fadeout){
        while(selfImg.color.a < 1){
            selfImg.color = new Color(selfImg.color.r, selfImg.color.g, selfImg.color.b, (selfImg.color.a) + (fadeSpeed * Time.deltaTime));
            yield return null;
        }
    }else{
        while(selfImg.color.a > 0){
            selfImg.color = new Color(selfImg.color.r, selfImg.color.g, selfImg.color.b, (selfImg.color.a) - (fadeSpeed * Time.deltaTime));
            yield return null;
        }
    }
}


public void showInfoCard(){ //this is seperated in two because the script that calls this gets deleted right after, meaning it can't finish the coroutine, so a middle man is needed
    StartCoroutine(showInfoCardEnumerator());
}

IEnumerator showInfoCardEnumerator(){
    StartCoroutine(fade(true));
    yield return new WaitForSeconds(5f);
    StartCoroutine(fade(false));
}





public void backToMenuFade(float timeToWait = 0f){
    StartCoroutine(backToMenuFadeEnumerator(timeToWait));
}



IEnumerator backToMenuFadeEnumerator(float timeToWait){
    yield return new WaitForSeconds(timeToWait);
    StartCoroutine(fade(true, 3));
    yield return new WaitForSeconds(3f);
    SceneManager.LoadScene("MainMenuScene");
}





}//END CLASS
