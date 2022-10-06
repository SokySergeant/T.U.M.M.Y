using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBarScript : MonoBehaviour
{

public Image imgSource;

public Sprite[] healthSprites;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }





    public void updateHealth(int health){
        if(health >= 0){
            imgSource.sprite = healthSprites[health];
        }else{ //accounts for things doing more than 1 damage to you while you have 1 hp
            imgSource.sprite = healthSprites[0];
        }
        
    }






}//END CLASS