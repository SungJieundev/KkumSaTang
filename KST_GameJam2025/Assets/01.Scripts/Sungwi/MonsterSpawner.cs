using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("설정")]
    public GameObject[] monsterPrefabs;     // 생성 가능한 여러 몬스터 프리팹들
    public Transform[] movePoints;          // 몬스터가 이동할 지점들 (예: 1~4번)
    public float spawnInterval = 5f;        // 몬스터 생성 주기 (초)
    public float monsterSpeed = 2f;         // 몬스터 이동 속도

    void Start()
    {
        // 시작 시 유효성 검사
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

        // Start에서 코루틴 시작 → 몬스터 생성 루프
        StartCoroutine(SpawnMonsterLoop());
    }

    // 몬스터 생성 루프 코루틴
    IEnumerator SpawnMonsterLoop()
    {
        while (true)
        {
            // 지정된 첫 번째 포인트에서 몬스터 생성
            // 즉, 일정 시간 간격으로 계속 생성됨
            SpawnMonster(movePoints[0]);

            // 다음 생성까지 대기
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    //  몬스터 생성 함수
    void SpawnMonster(Transform spawnPoint)
    {
        // 랜덤한 몬스터 프리팹 선택
        int randomIndex = Random.Range(0, monsterPrefabs.Length);

        // 선택된 프리팹을 해당 위치에 생성
        GameObject monster = Instantiate(monsterPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);

        // 몬스터 이동 시작 (0번 포인트부터)
        StartCoroutine(MoveMonster(monster, 0));
    }

    //  몬스터가 지정된 포인트를 따라 계속 반복해서 이동하는 코루틴
    IEnumerator MoveMonster(GameObject monster, int startIndex)
    {
        int index = startIndex;

        while (monster != null)
        {
            // 다음 위치 포인트 계산 (1 → 2 → 3 → 4 → 다시 1 반복)
            int nextIndex = (index + 1) % movePoints.Length;
            Vector3 target = movePoints[nextIndex].position;

            // 현재 위치에서 다음 위치로 이동
            while (Vector3.Distance(monster.transform.position, target) > 0.05f)
            {
                if (monster == null) yield break; // 도중에 파괴되면 중단

                // 천천히 다음 위치로 이동
                monster.transform.position = Vector3.MoveTowards(
                    monster.transform.position,
                    target,
                    monsterSpeed * Time.deltaTime
                );

                yield return null; // 다음 프레임까지 대기
            }

            // 다음 목적지로 인덱스 이동
            index = nextIndex;
            yield return null;
        }
    }
}
