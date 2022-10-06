using UnityEngine.Audio;
using UnityEngine;
using System;

public class audioManagerScript : MonoBehaviour
{
    public audio[] sounds;

    // Start is called before the first frame update
    void Awake()
    {
        foreach(audio sound in sounds){
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    void Start(){
        playSound("music"); //continuous music
    }

    
    public void playSound(string name){
        audio s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
}//END CLASS
