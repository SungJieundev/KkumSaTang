using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("����")]
    public GameObject[] monsterPrefabs;     // ���� ������ ���� ���� �����յ�
    public Transform[] movePoints;          // ���Ͱ� �̵��� ��� ����Ʈ��
    public float spawnInterval = 5f;        // ���� ���� �ֱ� (��)
    public float monsterSpeed = 2f;         // ���� �̵� �ӵ�

    void Start()
    {
        // ��ȿ�� �˻�
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

        // ���� ���� ���� ����
        StartCoroutine(SpawnMonsterLoop());
    }

    IEnumerator SpawnMonsterLoop()
    {
        while (true)
        {
            SpawnMonster(movePoints[0]);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnMonster(Transform spawnPoint)
    {
        int randomIndex = Random.Range(0, monsterPrefabs.Length);
        GameObject monster = Instantiate(monsterPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);

        StartCoroutine(MoveMonster(monster, 0));
    }

    IEnumerator MoveMonster(GameObject monster, int startIndex)
    {
        int index = startIndex;

        while (monster != null && !monster.Equals(null))
        {
            int nextIndex = (index + 1) % movePoints.Length;
            Vector3 target = movePoints[nextIndex].position;

            // ���Ͱ� target �������� �̵�
            while (monster != null && !monster.Equals(null) && Vector3.Distance(monster.transform.position, target) > 0.05f)
            {
                monster.transform.position = Vector3.MoveTowards(
                    monster.transform.position,
                    target,
                    monsterSpeed * Time.deltaTime
                );

                yield return null;
            }

            if (monster == null || monster.Equals(null))
                yield break;

            index = nextIndex;
            yield return null;
        }
    }
}
