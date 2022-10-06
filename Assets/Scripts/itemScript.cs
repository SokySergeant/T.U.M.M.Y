using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemScript : MonoBehaviour
{

public string itemType;
public string itemName;
public GameObject self;

public void removeItem(){
    Object.Destroy(self);
}



}//END CLASS
