using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHp : MonoBehaviour
{
    public float maxHp = 100;
    public float currentHp;

    public int rewardMoney = 10000;      // �⺻ ����ݾ�
    private MoneyManager moneyManager;   // �� �Ŵ��� ����

    void Start()
    {
        currentHp = maxHp;

        // ���� MoneyManager �ڵ� ã��
        moneyManager = FindObjectOfType<MoneyManager>();

        if (moneyManager == null)
        {
            Debug.LogWarning("MoneyManager�� ���� �����ϴ�!");
        }
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        Debug.Log($"{gameObject.name} �ǰ�: ���� HP {currentHp}");

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
            Debug.Log($"���� +{rewardMoney}��! ���� ��: {moneyManager.currentMoney}");
        }
        AudioManager.Instance.PlaySystem("MonsterDie");

        Destroy(gameObject);
    }
}
