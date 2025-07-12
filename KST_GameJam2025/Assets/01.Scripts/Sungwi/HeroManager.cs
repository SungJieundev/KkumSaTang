using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Ư�� ���� ������ �����ϴ� Ŭ����
[System.Serializable]
public class HeroUpgradePair
{
    public string requiredA;           // ù ��° �ʿ��� ���� �̸�
    public string requiredB;           // �� ��° �ʿ��� ���� �̸�
    public GameObject resultPrefab;    // ������ ����� ���� ������
}
public class HeroManager : MonoBehaviour
{
    [Header("������")]
    public GameObject[] highLevelPrefabs;
    public List<HeroUpgradePair> specialUpgradePairs;

    //[Header("���׷��̵� ��ġ")]
    //public Transform spawnPoint;

    public void TryUpgradeLowToHigh()
    {
        string[] lowNames = {
        "Low_Croissant", "Low_RollBread", "Low_Baguette", "Low_WhiteBread"
    };

        Dictionary<string, List<GameObject>> groups = new Dictionary<string, List<GameObject>>();

        GameObject[] allHeroes = GameObject.FindGameObjectsWithTag("Hero");

        // �̸����� �׷�ȭ
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

        // ���� �̸����� 3�� �̻� �ִ� �׷� ã��
        foreach (var pair in groups)
        {
            if (pair.Value.Count >= 3)
            {
                // 3�� ����
                for (int i = 0; i < 3; i++)
                {
                    PoolManager.Instance.HeroDeSpawn(pair.Value[i]);
                    //Destroy(pair.Value[i]);
                }

                // ���̷��� ���� ����
                int r = Random.Range(0, highLevelPrefabs.Length);
                
                //Instantiate(highLevelPrefabs[r], spawnPoint.position, Quaternion.identity);
                RandomGacha.Instance.SpawnHero(highLevelPrefabs[r].gameObject);
                //PoolManager.Instance.HeroSpawn(highLevelPrefabs[r].gameObject, spawnPoint);
                
                Debug.Log(pair.Key + " 3���� High ���� ���� �Ϸ�");
                return;
            }
        }

        Debug.Log("���� ������ Low ������ 3�� �̻� �ʿ��մϴ�.");
    }

    public void TryUpgradeHighToSpecial()
    {
        foreach (var pair in specialUpgradePairs)
        {
            GameObject unitA = GameObject.Find(pair.requiredA);
            GameObject unitB = GameObject.Find(pair.requiredB);

            if (unitA != null && unitB != null)
            {
                //Destroy(unitA);
                PoolManager.Instance.HeroDeSpawn(unitA);
                PoolManager.Instance.HeroDeSpawn(unitB);
                //Destroy(unitB);

                
                //Instantiate(pair.resultPrefab, spawnPoint.position, Quaternion.identity);
                //PoolManager.Instance.HeroSpawn(pair.resultPrefab, spawnPoint);
                RandomGacha.Instance.SpawnHero(pair.resultPrefab);
                
                Debug.Log($"{pair.requiredA} + {pair.requiredB} �� Special ����");
                return;
            }
        }

        Debug.Log("Ư�� ������ ã�� �� �����ϴ�.");
    }

    private List<GameObject> FindObjectsWithNames(string[] names)
    {
        List<GameObject> result = new List<GameObject>();

        foreach (string name in names)
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("Hero");
            foreach (GameObject obj in objs)
            {
                if (obj.name.StartsWith(name))  // ������ �̸��� (Clone) ���� �� ����
                {
                    result.Add(obj);
                }
            }
        }

        return result;
    }
}
