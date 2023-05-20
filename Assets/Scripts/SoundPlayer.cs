using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private EventReference music;
    
    [SerializeField] private EventReference eat;
    [SerializeField] private EventReference fly;
    [SerializeField] private EventReference birth;
    [SerializeField] private EventReference lay_egg;
    [SerializeField] private EventReference death;
    [SerializeField] private EventReference item_spawn;
    [SerializeField] private EventReference SFX_Failed;
    [SerializeField] private EventReference SFX_Success;
    
    
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
