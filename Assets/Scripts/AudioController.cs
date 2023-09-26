using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    
    public static AudioClip gameMusic, menuSelect, menuChange, playerDeath;
    static AudioSource source;

    void Start()
    {
        
        gameMusic = Resources.Load<AudioClip>("GameMusic");
        gameMusic = Resources.Load<AudioClip>("MenuSelect");
        gameMusic = Resources.Load<AudioClip>("MenuChange");
        gameMusic = Resources.Load<AudioClip>("PlayerDeath");
        source = GetComponent<AudioSource>();
        source.Play();

    }

    public void ButtonPress(AudioClip menuSelect)
    {
        
        source = GetComponent<AudioSource>();
        source.PlayOneShot(menuSelect, 1.75f);
    }


    public void ButtonChange(AudioClip menuChange)
    {
        
        source = GetComponent<AudioSource>();
        source.PlayOneShot(menuChange, 1.75f);
    }

}
