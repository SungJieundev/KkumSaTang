using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("����")]
    public GameObject[] monsterPrefabs;     // ���� ������ ���� ���� �����յ�
    public Transform[] movePoints;          // ���Ͱ� �̵��� ������ (��: 1~4��)
    public float spawnInterval = 5f;        // ���� ���� �ֱ� (��)
    public float monsterSpeed = 2f;         // ���� �̵� �ӵ�

    void Start()
    {
        // ���� �� ��ȿ�� �˻�
        if (monsterPrefabs.Length == 0)
        {
            Debug.LogError("���� �������� �������� �ʾҽ��ϴ�!");
            return;
        }

        if (movePoints.Length < 2)
        {
            Debug.LogError("movePoints�� �ּ� 2�� �̻��� �ʿ��մϴ�!");
            return;
        }

        // Start���� �ڷ�ƾ ���� �� ���� ���� ����
        StartCoroutine(SpawnMonsterLoop());
    }

    // ���� ���� ���� �ڷ�ƾ
    IEnumerator SpawnMonsterLoop()
    {
        while (true)
        {
            // ������ ù ��° ����Ʈ���� ���� ����
            // ��, ���� �ð� �������� ��� ������
            SpawnMonster(movePoints[0]);

            // ���� �������� ���
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    //  ���� ���� �Լ�
    void SpawnMonster(Transform spawnPoint)
    {
        // ������ ���� ������ ����
        int randomIndex = Random.Range(0, monsterPrefabs.Length);

        // ���õ� �������� �ش� ��ġ�� ����
        GameObject monster = Instantiate(monsterPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);

        // ���� �̵� ���� (0�� ����Ʈ����)
        StartCoroutine(MoveMonster(monster, 0));
    }

    //  ���Ͱ� ������ ����Ʈ�� ���� ��� �ݺ��ؼ� �̵��ϴ� �ڷ�ƾ
    IEnumerator MoveMonster(GameObject monster, int startIndex)
    {
        int index = startIndex;

        while (monster != null)
        {
            // ���� ��ġ ����Ʈ ��� (1 �� 2 �� 3 �� 4 �� �ٽ� 1 �ݺ�)
            int nextIndex = (index + 1) % movePoints.Length;
            Vector3 target = movePoints[nextIndex].position;

            // ���� ��ġ���� ���� ��ġ�� �̵�
            while (Vector3.Distance(monster.transform.position, target) > 0.05f)
            {
                if (monster == null) yield break; // ���߿� �ı��Ǹ� �ߴ�

                // õõ�� ���� ��ġ�� �̵�
                monster.transform.position = Vector3.MoveTowards(
                    monster.transform.position,
                    target,
                    monsterSpeed * Time.deltaTime
                );

                yield return null; // ���� �����ӱ��� ���
            }

            // ���� �������� �ε��� �̵�
            index = nextIndex;
            yield return null;
        }
    }
}
