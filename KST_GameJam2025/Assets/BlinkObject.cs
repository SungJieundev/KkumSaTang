using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlinkObject : MonoBehaviour
{
    public float duration = 1f;  // 한 번 깜빡이는데 걸리는 시간
    private TextMeshProUGUI tmp;
    private float timer = 0f;
    private bool fadingOut = true;

    void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        if (tmp == null)
        {
            Debug.LogError("BlinkFadeTMP는 TextMeshProUGUI 컴포넌트가 필요합니다!");
            enabled = false;
        }
    }

    void Update()
    {
        timer += Time.unscaledDeltaTime;
        float t = timer / duration;

        Color color = tmp.color;
        if (fadingOut)
            color.a = Mathf.Lerp(1f, 0f, t);
        else
            color.a = Mathf.Lerp(0f, 1f, t);

        tmp.color = color;

        if (t >= 1f)
        {
            fadingOut = !fadingOut;
            timer = 0f;
        }
    }
    
}
