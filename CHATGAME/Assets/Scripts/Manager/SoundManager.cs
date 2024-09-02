using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource Audio;

    void Start()
    {
        SoundPlay();
    }

    public void SoundPlay()
    {
        if(Audio != null && !Audio.isPlaying)
        {
            Audio.Play();
        }
    }

    public void SoundSetting(float val)
    {
        Audio.volume = val;
    }

    
}
