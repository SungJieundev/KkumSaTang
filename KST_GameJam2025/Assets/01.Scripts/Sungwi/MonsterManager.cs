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
    public Text monsterCountText;

    // 이벤트 정의
    public event Action OnMonsterWarning;      // 30마리 남았을 때
    public event Action OnMonsterNearLimit;    // 10마리 남았을 때
    public event Action OnMonsterLimitExceeded; // 초과했을 때

    private bool warningTriggered = false;
    private bool nearLimitTriggered = false;
    private bool exceededTriggered = false;

    void Update()
    {
        int currentCount = GameObject.FindGameObjectsWithTag("Monster").Length;

        // UI 업데이트
        if (monsterCountText != null)
        {
            monsterCountText.text = currentCount + " / " + maxMonsterCount;
        }

        int remaining = maxMonsterCount - currentCount;

        // 위험 경고 (30마리 이하)
        if (!warningTriggered && remaining <= 30)
        {
            warningTriggered = true;
            Debug.Log("위험: 몬스터가 30마리 이하로 남았습니다!");
            OnMonsterWarning?.Invoke();
        }

        // 거의 다 참 (10마리 이하)
        if (!nearLimitTriggered && remaining <= 10)
        {
            nearLimitTriggered = true;
            Debug.Log(" 경고: 몬스터가 거의 다 찼습니다!");
            OnMonsterNearLimit?.Invoke();
        }

        // 초과
        if (!exceededTriggered && currentCount >= maxMonsterCount)
        {
            exceededTriggered = true;
            Debug.Log("실패: 최대 몬스터 수를 초과했습니다.");
            OnMonsterLimitExceeded?.Invoke();
        }
    }
}
