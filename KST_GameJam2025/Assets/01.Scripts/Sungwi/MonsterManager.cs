using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System; // �̺�Ʈ��

public class MonsterManager : MonoBehaviour
{

    [Header("���� �� ����")]
    public int maxMonsterCount = 100;

    [Header("UI")]
    public Text monsterCountText;

    // �̺�Ʈ ����
    public event Action OnMonsterWarning;      // 30���� ������ ��
    public event Action OnMonsterNearLimit;    // 10���� ������ ��
    public event Action OnMonsterLimitExceeded; // �ʰ����� ��

    private bool warningTriggered = false;
    private bool nearLimitTriggered = false;
    private bool exceededTriggered = false;

    void Update()
    {
        int currentCount = GameObject.FindGameObjectsWithTag("Monster").Length;

        // UI ������Ʈ
        if (monsterCountText != null)
        {
            monsterCountText.text = currentCount + " / " + maxMonsterCount;
        }

        int remaining = maxMonsterCount - currentCount;

        // ���� ��� (30���� ����)
        if (!warningTriggered && remaining <= 30)
        {
            warningTriggered = true;
            Debug.Log("����: ���Ͱ� 30���� ���Ϸ� ���ҽ��ϴ�!");
            OnMonsterWarning?.Invoke();
        }

        // ���� �� �� (10���� ����)
        if (!nearLimitTriggered && remaining <= 10)
        {
            nearLimitTriggered = true;
            Debug.Log(" ���: ���Ͱ� ���� �� á���ϴ�!");
            OnMonsterNearLimit?.Invoke();
        }

        // �ʰ�
        if (!exceededTriggered && currentCount >= maxMonsterCount)
        {
            exceededTriggered = true;
            Debug.Log("����: �ִ� ���� ���� �ʰ��߽��ϴ�.");
            OnMonsterLimitExceeded?.Invoke();
        }
    }
}
