using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyScript : MonoBehaviour
{

public Animator selfAnim;
public string keyColor;

    // Start is called before the first frame update
    void Start()
    {
        switch(keyColor){
            case "lightBlue":
                selfAnim.Play("lightBlueKeyAnimation");
                break;
            case "yellow":
                selfAnim.Play("yellowKeyAnimation");
                break;
        }
    }

}
