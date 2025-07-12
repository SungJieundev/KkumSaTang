using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// 특수 조합 정보를 저장하는 클래스
[System.Serializable]
public class HeroUpgradePair
{
    public string requiredA;           // 첫 번째 필요한 유닛 이름
    public string requiredB;           // 두 번째 필요한 유닛 이름
    public GameObject resultPrefab;    // 생성될 스페셜 유닛 프리팹
}
public class HeroManager : MonoBehaviour
{
    [Header("프리팹")]
    public GameObject[] highLevelPrefabs;
    public List<HeroUpgradePair> specialUpgradePairs;

    [Header("업그레이드 위치")]
    public Transform spawnPoint;

    public void TryUpgradeLowToHigh()
    {
        string[] lowNames = {
        "Low_Croissant", "Low_RollBread", "Low_Baguette", "Low_WhiteBread"
    };

        Dictionary<string, List<GameObject>> groups = new Dictionary<string, List<GameObject>>();

        GameObject[] allHeroes = GameObject.FindGameObjectsWithTag("Hero");

        // 이름별로 그룹화
        foreach (string name in lowNames)
        {
            groups[name] = new List<GameObject>();
        }

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

        // 같은 이름으로 3개 이상 있는 그룹 찾기
        foreach (var pair in groups)
        {
            if (pair.Value.Count >= 3)
            {
                // 3개 제거
                for (int i = 0; i < 3; i++)
                    Destroy(pair.Value[i]);

                // 하이레벨 랜덤 생성
                int r = Random.Range(0, highLevelPrefabs.Length);
                Instantiate(highLevelPrefabs[r], spawnPoint.position, Quaternion.identity); //생성

                Debug.Log(pair.Key + " 3개로 High 유닛 생성 완료");
                return;
            }
        }

        Debug.Log("같은 종류의 Low 유닛이 3개 이상 필요합니다.");
    }

    public void TryUpgradeHighToSpecial()
    {
        foreach (var pair in specialUpgradePairs)
        {
            GameObject unitA = GameObject.Find(pair.requiredA);
            GameObject unitB = GameObject.Find(pair.requiredB);

            if (unitA != null && unitB != null)
            {
                Destroy(unitA);
                Destroy(unitB);

                Instantiate(pair.resultPrefab, spawnPoint.position, Quaternion.identity);
                Debug.Log($"{pair.requiredA} + {pair.requiredB} → Special 생성");
                return;
            }
        }

        Debug.Log("특수 조합을 찾을 수 없습니다.");
    }

    private List<GameObject> FindObjectsWithNames(string[] names)
    {
        List<GameObject> result = new List<GameObject>();

        foreach (string name in names)
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("Hero");
            foreach (GameObject obj in objs)
            {
                if (obj.name.StartsWith(name))  // 프리팹 이름에 (Clone) 있을 수 있음
                {
                    result.Add(obj);
                }
            }
        }

        return result;
    }
}
