using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GachaEventFeedback : MonoBehaviour
{
    public AudioSource bangSound;
    public GameObject burntImagePanel;

    private void Start()
    {
        bangSound = GetComponent<AudioSource>();
    }

    public void OnBangEventHandler()
    {
        bangSound.Play(); //꽝 효과음 재생
        
        Sequence seq = DOTween.Sequence();

        seq.Append(burntImagePanel.transform.DOScale(new Vector3(0.6f, 0.6f), 0.4f))
            .AppendCallback(() =>
            {
                burntImagePanel.SetActive(false); // 패널 끄고
                burntImagePanel.transform.localScale = Vector3.zero; // 크기 줄여줌
            });
    }
    
    
    
    
}
