using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class HeroUpgradePair
{
    public string requiredA;           // 첫 번째 필요한 유닛 이름
    public string requiredB;           // 두 번째 필요한 유닛 이름
    public GameObject resultPrefab;    // 조합 결과로 나올 Special 유닛 프리팹
}

public class HeroManager : MonoBehaviour
{
    [Header("High 레벨 프리팹 목록")]
    public GameObject[] highLevelPrefabs;

    [Header("Special 업그레이드 조합 목록")]
    public List<HeroUpgradePair> specialUpgradePairs;

    /// <summary>
    /// Low 레벨 유닛 3개를 모으면 High 레벨 유닛으로 업그레이드
    /// </summary>
    public void TryUpgradeLowToHigh()
    {
        // 업그레이드 가능한 Low 유닛 이름들
        string[] lowNames = {
            "Low_Croissant", "Low_RollBread", "Low_Baguette", "Low_WhiteBread"
        };

        // 이름별 유닛을 분류할 그룹
        Dictionary<string, List<GameObject>> groups = new Dictionary<string, List<GameObject>>();

        GameObject[] allHeroes = GameObject.FindGameObjectsWithTag("Hero");

        // 이름 기준으로 그룹 초기화
        foreach (string name in lowNames)
        {
            groups[name] = new List<GameObject>();
        }

        // 각 Low 유닛들을 해당 그룹에 분류
        foreach (GameObject obj in allHeroes)
        {
            foreach (string name in lowNames)
            {
                if (obj.name.StartsWith(name))
                {
                    groups[name].Add(obj);
                    break;
                }
            }
        }

        // 그룹 중에 3개 이상 존재하는 유닛이 있으면 업그레이드
        foreach (var pair in groups)
        {
            if (pair.Value.Count >= 3)
            {
                // 기존 Low 유닛 3개 제거 (풀링 방식 사용)
                for (int i = 0; i < 3; i++)
                {
                    PoolManager.Instance.HeroDeSpawn(pair.Value[i]);
                }

                // 랜덤 High 유닛 생성
                int r = Random.Range(0, highLevelPrefabs.Length);
                RandomGacha.Instance.SpawnHero(highLevelPrefabs[r].gameObject);

                Debug.Log(pair.Key + " 유닛 3개가 합쳐져 High 레벨 유닛 생성됨!");
                return;
            }
        }

        Debug.Log("Low 레벨 유닛이 3개 이상 존재하지 않습니다.");
    }

    /// <summary>
    /// 특정 High 유닛 2개가 있으면 Special 유닛으로 업그레이드
    /// </summary>
    public void TryUpgradeHighToSpecial()
    {
        foreach (var pair in specialUpgradePairs)
        {
            GameObject unitA = GameObject.Find(pair.requiredA);
            GameObject unitB = GameObject.Find(pair.requiredB);

            if (unitA != null && unitB != null)
            {
                // 기존 유닛 제거 (풀링 방식)
                PoolManager.Instance.HeroDeSpawn(unitA);
                PoolManager.Instance.HeroDeSpawn(unitB);

                // Special 유닛 생성
                RandomGacha.Instance.SpawnHero(pair.resultPrefab);

                Debug.Log($"{pair.requiredA} + {pair.requiredB} → Special 유닛 생성!");
                return;
            }
        }

        Debug.Log("조건에 맞는 Special 조합을 찾을 수 없습니다.");
    }

    /// <summary>
    /// 이름 배열에 해당하는 유닛들을 모두 찾아 리스트로 반환
    /// </summary>
    private List<GameObject> FindObjectsWithNames(string[] names)
    {
        List<GameObject> result = new List<GameObject>();

        foreach (string name in names)
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("Hero");
            foreach (GameObject obj in objs)
            {
                if (obj.name.StartsWith(name))  // (Clone) 포함되어 있어도 OK
                {
                    result.Add(obj);
                }
            }
        }

        return result;
    }
}
