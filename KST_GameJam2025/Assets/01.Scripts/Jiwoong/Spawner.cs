using UnityEngine;


public class Spawner : MonoBehaviour
{
    public GameObject[] prefabsToSpawn; // 4�� �������� �ν����Ϳ� �ֱ�

    public void SpawnObject(Vector2 position)
    {
        if (prefabsToSpawn.Length == 0)
        {
            Debug.LogWarning("������ ����Ʈ�� ����ֽ��ϴ�!");
            return;
        }

        int randomIndex = Random.Range(0, prefabsToSpawn.Length); // 0���� 3���� ����
        GameObject selectedPrefab = prefabsToSpawn[randomIndex];

        GameObject obj = Instantiate(selectedPrefab, position, Quaternion.identity);
        ObjectManager.Instance.RegisterObject(obj); // ���û���
    }
}

