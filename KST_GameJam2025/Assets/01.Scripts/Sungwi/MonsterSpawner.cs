using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("몬스터 프리팹")]
    public GameObject[] monsterPrefabs;     // 0~4: 반죽 뭉치, 탄 식빵, 바게트, 모닝빵, 단팥빵

    [Header("이동 경로")]
    public Transform[] movePoints;

    [Header("설정")]
    public float spawnDelay = 1f;          // 몬스터 간 생성 간격
    public float waveDuration = 30f;       // 웨이브당 시간
    public float monsterSpeed = 2f;

    private bool isSpawning = false;

    void Start()
    {
        StartCoroutine(WaveLoop());
    }

    IEnumerator WaveLoop()
    {
        while (GameManager.Instance.waveCount <= 10)
        {
            int wave = GameManager.Instance.waveCount;
            Debug.Log($"웨이브 {wave} 시작");

            yield return StartCoroutine(SpawnWave(wave));

            Debug.Log($"웨이브 {wave} 완료, 다음 웨이브로...");

            yield return new WaitForSeconds(waveDuration); // 웨이브 지속시간 대기

            GameManager.Instance.waveCount++;
        }

        Debug.Log("모든 웨이브 완료!");
    }

    IEnumerator SpawnWave(int wave)
    {
        isSpawning = true;

        List<int> spawnList = GetSpawnListForWave(wave);

        foreach (int monsterIndex in spawnList)
        {
            SpawnMonster(monsterPrefabs[monsterIndex]);
            yield return new WaitForSeconds(spawnDelay);
        }

        isSpawning = false;
    }

    void SpawnMonster(GameObject prefab)
    {
        GameObject monster = Instantiate(prefab, movePoints[0].position, Quaternion.identity);
        StartCoroutine(MoveMonster(monster, 0));
    }

    IEnumerator MoveMonster(GameObject monster, int startIndex)
    {
        int index = startIndex;

        while (monster != null)
        {
            int nextIndex = (index + 1) % movePoints.Length;
            Vector3 target = movePoints[nextIndex].position;

            while (monster != null && Vector3.Distance(monster.transform.position, target) > 0.05f)
            {
                monster.transform.position = Vector3.MoveTowards(
                    monster.transform.position,
                    target,
                    monsterSpeed * Time.deltaTime
                );
                yield return null;
            }

            index = nextIndex;
            yield return null;
        }
    }

    List<int> GetSpawnListForWave(int wave)
    {
        List<int> result = new List<int>();

        switch (wave)
        {
            case 1: result.AddRange(RepeatIndex(0, 6)); break;
            case 2: result.AddRange(RepeatIndex(0, 4)); result.AddRange(RepeatIndex(1, 2)); break;
            case 3: result.AddRange(RepeatIndex(0, 3)); result.AddRange(RepeatIndex(1, 3)); result.Add(2); break;
            case 4: result.AddRange(RepeatIndex(1, 4)); result.AddRange(RepeatIndex(2, 2)); break;
            case 5: result.AddRange(RepeatIndex(2, 3)); result.Add(3); break;
            case 6: result.AddRange(RepeatIndex(1, 2)); result.AddRange(RepeatIndex(2, 2)); result.AddRange(RepeatIndex(3, 2)); break;
            case 7: result.AddRange(RepeatIndex(2, 3)); result.AddRange(RepeatIndex(3, 3)); break;
            case 8: result.AddRange(RepeatIndex(3, 4)); break;
            case 9: result.AddRange(RepeatIndex(3, 3)); result.Add(4); break;
            case 10: result.AddRange(RepeatIndex(4, 2)); break;
        }

        return result;
    }

    List<int> RepeatIndex(int index, int count)
    {
        List<int> list = new List<int>();
        for (int i = 0; i < count; i++) list.Add(index);
        return list;
    }
}
