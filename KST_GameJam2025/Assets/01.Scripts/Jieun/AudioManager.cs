using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance = null;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<AudioManager>();

            return instance;
        }
    }

    public List<AudioClip> clips = new List<AudioClip>();
    private Dictionary<string, AudioClip> clipPool = new Dictionary<string, AudioClip>();

    public AudioSource bgmPlayer = null;
    public AudioSource systemPlayer = null;

    private void Awake()
    {
        foreach (AudioClip clip in clips)
            CreateAudioPool(clip);
    }

    public void PlayBGM(string clipName) => PlayAudio(clipName, bgmPlayer);
    public void PauseBGM() => bgmPlayer.Pause();
    public void PlaySystem(string clipName) => PlaySFX(clipName, systemPlayer);
    public void PauseSystem() => systemPlayer.Pause();

    public void PlayAudio(string clipName, AudioSource player)
    {
        if (!clipPool.ContainsKey(clipName))
        {
            Debug.LogWarning("Current name of auido clip doesnt exist");
            return;
        }

        player.clip = clipPool[clipName];

        player.Play();
    }
    
    public void PlaySFX(string clipName, AudioSource player)
    {
        if (!clipPool.TryGetValue(clipName, out var clip))
        {
            Debug.LogWarning($"[{clipName}] 오디오 클립이 없습니다!");
            return;
        }

        systemPlayer.PlayOneShot(clip);   // 겹쳐 재생 O
    }

    private void CreateAudioPool(AudioClip clip)
    {
        if (clipPool.ContainsKey(clip.name))
        {
            Debug.LogWarning("Current name of audio clip is already exist");
            return;
        }

        clipPool.Add(clip.name, clip);
    }
}