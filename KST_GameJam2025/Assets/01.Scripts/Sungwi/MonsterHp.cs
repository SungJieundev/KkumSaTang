using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHp : MonoBehaviour
{
    public float maxHp = 100;
    public float currentHp;

    public int rewardMoney = 10000;      // 기본 보상금액
    private MoneyManager moneyManager;   // 돈 매니저 참조

    void Start()
    {
        currentHp = maxHp;

        // 씬에 MoneyManager 자동 찾기
        moneyManager = FindObjectOfType<MoneyManager>();

        if (moneyManager == null)
        {
            Debug.LogWarning("MoneyManager가 씬에 없습니다!");
        }
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        Debug.Log($"{gameObject.name} 피격: 현재 HP {currentHp}");

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (moneyManager != null)
        {
            moneyManager.AddMoney(rewardMoney);
            Debug.Log($"보상 +{rewardMoney}원! 현재 돈: {moneyManager.currentMoney}");
        }

        Destroy(gameObject);
    }
}
