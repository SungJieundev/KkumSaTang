using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public static GameManager Instance;
    
    private void Awake()
    {
        
        if(Instance == null) Instance = this;
        
        Screen.SetResolution(1920, 1080, true);
    }
}
