using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System; // �̺�Ʈ��

public class MonsterManager : MonoBehaviour
{

    public GameObject FailPanel;
    [Header("���� �� ����")]
    public int maxMonsterCount = 100;

    [Header("UI")]
    public Image monsterGaugeImage;     // �������ٿ� �̹��� (Image Type: Filled�� ����)

    // �̺�Ʈ ����
    public event Action OnMonsterWarning;        // 30���� ������ ��
    public event Action OnMonsterNearLimit;      // 10���� ������ ��
    public event Action OnMonsterLimitExceeded;  // �ִ� �ʰ� ��

    private bool warningTriggered = false;
    private bool nearLimitTriggered = false;
    private bool exceededTriggered = false;

    void Update()
    {
        int currentCount = GameObject.FindGameObjectsWithTag("Monster").Length;

        // �������� ������Ʈ (0~1 ���� ����)
        if (monsterGaugeImage != null)
        {
            float fillRatio = Mathf.Clamp01((float)currentCount / maxMonsterCount);
            monsterGaugeImage.fillAmount = fillRatio;
        }

        int remaining = maxMonsterCount - currentCount;

        // �̺�Ʈ Ʈ����
        if (!warningTriggered && remaining <= 30)
        {
            warningTriggered = true;
            Debug.Log("����: ���Ͱ� 30���� ���Ϸ� ���ҽ��ϴ�!");
            OnMonsterWarning?.Invoke();
        }

        if (!nearLimitTriggered && remaining <= 10)
        {
            nearLimitTriggered = true;
            Debug.Log("���: ���Ͱ� ���� �� á���ϴ�!");
            OnMonsterNearLimit?.Invoke();
        }

        if (!exceededTriggered && currentCount >= maxMonsterCount)
        {
            FailPanel.SetActive(true);
            exceededTriggered = true;
            Debug.Log("����: �ִ� ���� ���� �ʰ��߽��ϴ�.");
            OnMonsterLimitExceeded?.Invoke();

            // ��: ���ӿ��� ó�� �߰� ����
            // GameOver();
        }
    }

    // �ʿ� �� ���ӿ��� ó�� �Լ�
    /*
    private void GameOver()
    {
        // ���ӿ��� UI ����, �ð� ���߱� ��
        Debug.Log("���� ����!");
    }
    */
}
