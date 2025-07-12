using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("웨이브 설정")]
    public int waveCount = 1; // 0이 아니라 1부터 시작해야 함
    public int maxWaves = 10;
    public float waveDuration = 60f; // 각 웨이브 시간 (초)

    [Header("UI")]
    public TMP_Text waveText;      // 웨이브 표시 텍스트
    public TMP_Text timerText;     // 남은 시간 표시 텍스트
    public CanvasGroup waveCanvasGroup; // Wave 텍스트 페이드용

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
        }

        // 모든 웨이브 종료 시 처리 (옵션)
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

        yield return new WaitForSeconds(2f); // 중간 표시 시간

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
