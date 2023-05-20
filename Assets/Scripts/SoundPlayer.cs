using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private EventReference music1;
    [SerializeField] private EventReference music2;
    [SerializeField] private EventReference music3;
    
    [SerializeField] private EventReference eat;
    [SerializeField] private EventReference fly;
    [SerializeField] private EventReference birth;
    [SerializeField] private EventReference lay_egg;
    [SerializeField] private EventReference death;
    [SerializeField] private EventReference item_spawn;
    [SerializeField] private EventReference SFX_Failed;
    [SerializeField] private EventReference SFX_Success;
    [SerializeField] private EventReference Atmo;

    public static SoundPlayer Instance;
    private EventInstance musicInstance;
    private EventInstance flySoundInstance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        RuntimeManager.PlayOneShot(Atmo);
        RuntimeManager.PlayOneShot(music1); //remove when different models are implemented
    }

    public void StopMusic()
    {
        musicInstance.stop(STOP_MODE.ALLOWFADEOUT);
    }

    public void PlayMusic1()
    {
        musicInstance.stop(STOP_MODE.IMMEDIATE);
        musicInstance = RuntimeManager.CreateInstance(music1);
        musicInstance.start();
    }
    
    public void PlayMusic2()
    {
        musicInstance.stop(STOP_MODE.IMMEDIATE);
        musicInstance = RuntimeManager.CreateInstance(music2);
        musicInstance.start();
    }
    
    public void PlayMusic3()
    {
        musicInstance.stop(STOP_MODE.IMMEDIATE);
        musicInstance = RuntimeManager.CreateInstance(music3);
        musicInstance.start();
    }

    public void StartFlySound()
    {
        flySoundInstance.stop(STOP_MODE.IMMEDIATE);
        flySoundInstance = RuntimeManager.CreateInstance(fly);
        flySoundInstance.start();
    }

    public void StopFlySound()
    {
        flySoundInstance.stop(STOP_MODE.ALLOWFADEOUT);
    }
}
