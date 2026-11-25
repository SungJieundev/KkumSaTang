using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject WinPanel;
    
    public bool IsSlotFull = false;

    [Header("���̺� ����")]
    public int waveCount = 1; // 0�� �ƴ϶� 1���� �����ؾ� ��
    public int maxWaves = 10;
    public float waveDuration = 60f; // �� ���̺� �ð� (��)

    [Header("UI")]
    public TMP_Text waveText;      // ���̺� ǥ�� �ؽ�Ʈ
    public TMP_Text timerText;     // ���� �ð� ǥ�� �ؽ�Ʈ
    public CanvasGroup waveCanvasGroup; // Wave �ؽ�Ʈ ���̵��

    private float currentTimer = 0f;
    private bool waveActive = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        Screen.SetResolution(1920, 1080, true);
    }

    private void Start()
    {
        StartCoroutine(WaveLoop());
        AudioManager.Instance.PlayBGM("GameBGM");
    }

    private void Update()
    {
        if (waveCount > 10)
        {
            WinPanel.SetActive((true));
        }
    }

    private IEnumerator WaveLoop()
    {
        for (waveCount = 1; waveCount <= maxWaves; waveCount++)
        {
            yield return StartCoroutine(ShowWaveIntro(waveCount));

            currentTimer = waveDuration;
            waveActive = true;

            while (currentTimer > 0)
            {
                currentTimer -= Time.deltaTime;
                UpdateTimerUI();
                yield return null;
            }

            waveActive = false;
            MoneyManager.Instance.AddDia(1);
        }

        // ��� ���̺� ���� �� ó�� (�ɼ�)
        timerText.text = "All Waves Cleared!";
    }

    private IEnumerator ShowWaveIntro(int waveNum)
    {
        waveText.text = $"Wave {waveNum}";
        waveCanvasGroup.alpha = 0f;
        waveCanvasGroup.gameObject.SetActive(true);

        float fadeTime = 0.5f;

        // Fade In
        float t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            waveCanvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeTime);
            yield return null;
        }

        yield return new WaitForSeconds(2f); // �߰� ǥ�� �ð�

        // Fade Out
        t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            waveCanvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeTime);
            yield return null;
        }

        waveCanvasGroup.gameObject.SetActive(false);
    }

    private void UpdateTimerUI()
    {
        int intTime = Mathf.CeilToInt(currentTimer);
        int minutes = intTime / 60;
        int seconds = intTime % 60;
        timerText.text = $"Time Left: {minutes:00}:{seconds:00}";
    }
}
