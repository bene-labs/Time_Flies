using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private EventReference music;

    private EventInstance musicInstance;
    void Start()
    {
        //RuntimeManager.PlayOneShot();
    }

    public void StopMusic()
    {
        musicInstance.stop(STOP_MODE.ALLOWFADEOUT);
    }
    
    
}
