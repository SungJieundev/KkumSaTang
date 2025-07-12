using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int waveCount = 0;
    
    TMP_Text waveText;

    public void WaveCountPlusEventHandler()
    {
        waveCount++;
        waveText.text = $"{waveCount.ToString()} Wave";
    }
    
    private void Awake()
    {
        
        if(Instance == null) Instance = this;
        
        Screen.SetResolution(1920, 1080, true);
    }
}
