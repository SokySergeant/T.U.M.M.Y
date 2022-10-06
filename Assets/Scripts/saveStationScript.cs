using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class saveStationScript : MonoBehaviour
{

    bool isLit = false;
    public Sprite unlitSprite;
    public Sprite litSprite;

    public SpriteRenderer selfSpriteRenderer;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    public IEnumerator lightUp(){
        if(!isLit){
            isLit = true;

            for (int i = 0; i < 7; i++){//light up pseudo animation
                selfSpriteRenderer.sprite = unlitSprite;
                yield return new WaitForSeconds(0.01f * i);
                selfSpriteRenderer.sprite = litSprite;
                yield return new WaitForSeconds(0.01f * i);
            }
        }
    }







}//END CLASS
