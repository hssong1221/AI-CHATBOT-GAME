using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource Audio;

    void Start()
    {
        SoundPlay();
        Audio.volume = PlayerPrefs.GetFloat("soundOpt", 0.3f);
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
