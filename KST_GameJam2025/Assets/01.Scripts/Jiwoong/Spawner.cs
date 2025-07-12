using UnityEngine;


public class Spawner : MonoBehaviour
{
    public GameObject[] prefabsToSpawn; // 4개 프리팹을 인스펙터에 넣기

    public void SpawnObject(Vector2 position)
    {
        if (prefabsToSpawn.Length == 0)
        {
            Debug.LogWarning("프리팹 리스트가 비어있습니다!");
            return;
        }

        int randomIndex = Random.Range(0, prefabsToSpawn.Length); // 0부터 3까지 랜덤
        GameObject selectedPrefab = prefabsToSpawn[randomIndex];

        GameObject obj = Instantiate(selectedPrefab, position, Quaternion.identity);
        ObjectManager.Instance.RegisterObject(obj); // 선택사항
    }
}

