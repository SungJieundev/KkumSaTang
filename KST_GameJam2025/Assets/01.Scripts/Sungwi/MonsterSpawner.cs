using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("설정")]
    public GameObject[] monsterPrefabs;     // 생성 가능한 여러 몬스터 프리팹들
    public Transform[] movePoints;          // 몬스터가 이동할 경로 포인트들
    public float spawnInterval = 5f;        // 몬스터 생성 주기 (초)
    public float monsterSpeed = 2f;         // 몬스터 이동 속도

    void Start()
    {
        // 유효성 검사
        if (monsterPrefabs.Length == 0)
        {
            Debug.LogError("몬스터 프리팹이 설정되지 않았습니다!");
            return;
        }

        if (movePoints.Length < 2)
        {
            Debug.LogError("movePoints는 최소 2개 이상이 필요합니다!");
            return;
        }

        // 몬스터 생성 루프 시작
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

            // 몬스터가 target 지점까지 이동
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
