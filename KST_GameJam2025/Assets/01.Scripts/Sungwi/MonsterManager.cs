using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System; // 이벤트용

public class MonsterManager : MonoBehaviour
{

    [Header("몬스터 수 설정")]
    public int maxMonsterCount = 100;

    [Header("UI")]
    public Image monsterGaugeImage;     // 게이지바용 이미지 (Image Type: Filled로 설정)

    // 이벤트 정의
    public event Action OnMonsterWarning;        // 30마리 남았을 때
    public event Action OnMonsterNearLimit;      // 10마리 남았을 때
    public event Action OnMonsterLimitExceeded;  // 최대 초과 시

    private bool warningTriggered = false;
    private bool nearLimitTriggered = false;
    private bool exceededTriggered = false;

    void Update()
    {
        int currentCount = GameObject.FindGameObjectsWithTag("Monster").Length;

        // 게이지바 업데이트 (0~1 사이 비율)
        if (monsterGaugeImage != null)
        {
            float fillRatio = Mathf.Clamp01((float)currentCount / maxMonsterCount);
            monsterGaugeImage.fillAmount = fillRatio;
        }

        int remaining = maxMonsterCount - currentCount;

        // 이벤트 트리거
        if (!warningTriggered && remaining <= 30)
        {
            warningTriggered = true;
            Debug.Log("위험: 몬스터가 30마리 이하로 남았습니다!");
            OnMonsterWarning?.Invoke();
        }

        if (!nearLimitTriggered && remaining <= 10)
        {
            nearLimitTriggered = true;
            Debug.Log("경고: 몬스터가 거의 다 찼습니다!");
            OnMonsterNearLimit?.Invoke();
        }

        if (!exceededTriggered && currentCount >= maxMonsterCount)
        {
            exceededTriggered = true;
            Debug.Log("실패: 최대 몬스터 수를 초과했습니다.");
            OnMonsterLimitExceeded?.Invoke();

            // 예: 게임오버 처리 추가 가능
            // GameOver();
        }
    }

    // 필요 시 게임오버 처리 함수
    /*
    private void GameOver()
    {
        // 게임오버 UI 띄우기, 시간 멈추기 등
        Debug.Log("게임 오버!");
    }
    */
}
