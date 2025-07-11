using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeContoller : MonoBehaviour
{
    private AudioMixer _audioMixer;
    public Slider volumeSlider;
    public string parameterName = "";

    private void Awake()
    {
        _audioMixer = GameManager.Instance.audioMixer;
    }

    public void OnValueChanged() {
        _audioMixer.SetFloat(parameterName,
            (volumeSlider.value <= volumeSlider.minValue) ? -80f : volumeSlider.value);
    }
}