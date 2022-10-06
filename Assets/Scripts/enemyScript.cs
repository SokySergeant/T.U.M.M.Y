using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour
{

public int maxHealth = 100;
public int enemyDamage = 20;
public int currentHealth;

/*
public GameObject self;
public Animator selfAnim;
*/






    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }//FIXEDUPDATE CLOSE




    void Update(){

    }//UPDATE CLOSE








/*
    public void takeDamage(int damage){
        currentHealth -= damage;
        
        StartCoroutine(takeDamageWaiter());
        
        if(currentHealth <= 0){
            //play dearth animation
            Object.Destroy(self);
        }
    }

    IEnumerator takeDamageWaiter(){
        selfAnim.SetBool("hurt", true);
        yield return new WaitForSeconds(0.2f);
        selfAnim.SetBool("hurt", false);
    }
*/













}//CLASS END