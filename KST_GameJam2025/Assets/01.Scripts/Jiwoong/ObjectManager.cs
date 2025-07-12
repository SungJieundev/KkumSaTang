using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.U2D.Animation;
public class ObjectManager : MonoBehaviour
{
    public static ObjectManager Instance; // �̱��� ������ ���� static �ν��Ͻ� ����
    public Spawner spawner; // ������Ʈ�� �����ϴ� ��ũ��Ʈ�� ����
    public Spawner2 spawner2; // ������ü �����ڵ�
    public GameObject upgradeButton; // ���׷��̵� ��ư ������Ʈ (��Ȱ��ȭ/Ȱ��ȭ��)
    public GameObject upgradeButtonSp; //�������� ��ư
    public GameObject Sp1;
    public GameObject Sp2;

    public List<GameObject> spawnedObjects = new List<GameObject>(); // ������ ������Ʈ���� �����ϴ� ����Ʈ

    private void Awake()
    {
        if (Instance == null)         // Instance�� ����ִٸ�
            Instance = this;          // ���� �ν��Ͻ��� �̱������� ����
        else
            Destroy(gameObject);      // �̹� �ٸ� �ν��Ͻ��� �ִٸ� �ߺ� ������ ���� ����
    }

    public void RegisterObject(GameObject obj)
    {
        spawnedObjects.Add(obj); // ������ ������Ʈ�� ����Ʈ�� ���

    }

    public void UnregisterObject(GameObject obj)
    {
        spawnedObjects.Remove(obj); // ������Ʈ�� ����Ʈ���� ����

    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // �����̽��ٸ� ������
        {
            spawner.SpawnObject(new Vector2(0, 0)); // (0,0) ��ġ�� ������Ʈ�� ����
            //����ĭ ���缭 ��������
            CheckUpgradeAvailable();
            CheckUpgradeAvailableSp();
        }



    }

    public void CheckUpgradeAvailable()
    {
        Dictionary<int, int> countById = new Dictionary<int, int>(); // ĳ���� ID���� ������ ���� ��ųʸ� ����

        foreach (GameObject obj in spawnedObjects) // ������ ������Ʈ�� ��� ��ȸ
        {
            CharacterInstance ci = obj.GetComponent<CharacterInstance>(); // CharacterInstance ������Ʈ ��������
            if (ci == null) continue; // ������ ����

            if (!countById.ContainsKey(ci.characterId)) // �ش� ID�� ������
                countById[ci.characterId] = 1;           // 1�� �ʱ�ȭ

            else
                countById[ci.characterId]++;             // �̹� ������ +1
            //id ���� 3�� �Ǹ� 0���� �ʱ�ȭ
        }

        foreach (var pair in countById) // ��ųʸ��� ��� �� Ȯ��
        {
            if (pair.Value >= 3) // ���� ID�� ĳ���Ͱ� 3�� �̻��̸�
            {
                upgradeButton.SetActive(true); // ���׷��̵� ��ư Ȱ��ȭ
                return; // ���� �����ϸ� �ٷ� ����
                //��ư�� �����ٸ� ��Ȱ��ȭ
                //�̹������� ������Ʈ Ȱ��ȭ ��Ȱ��ȭ
                //���� ĳ���� ��ġ�� ������ް�ü ��
            }
        }



        upgradeButton.SetActive(false); // ���� �����ϴ� ĳ���Ͱ� ������ ��ư ��Ȱ��ȭ
    }

    public void CheckUpgradeAvailableSp()
    {
        // ID���� ���� ���� Ȯ��
        bool has1 = false;
        bool has2 = false;
        bool has3 = false;

        foreach (GameObject obj in spawnedObjects)
        {
            CharacterInstance ci = obj.GetComponent<CharacterInstance>();
            if (ci == null) continue;

            if (ci.characterId == 1/*���ս� ¥�� ĳ���;��̵�*/) has1 = true;
            if (ci.characterId == 2/*ĳ���;��̵�*/) has2 = true;
            if (ci.characterId == 5/*ĳ���;��̵�*/) has3 = true;
        }

        if (has1 && has2 && has3)
            upgradeButtonSp.SetActive(true);
        else
            upgradeButtonSp.SetActive(false);
    }

    public void LvUp1()
    {
        Dictionary<int, List<GameObject>> idGroups = new Dictionary<int, List<GameObject>>();

        // 1. ���� ID���� �׷� �����
        foreach (GameObject obj in spawnedObjects)
        {
            CharacterInstance ci = obj.GetComponent<CharacterInstance>();
            if (ci == null) continue;

            int id = ci.characterId;

            if (!idGroups.ContainsKey(id))
                idGroups[id] = new List<GameObject>();

            idGroups[id].Add(obj);
        }

        // 2. ���׷��̵� ��� ã�� (3�� �̻��� �׷�)
        foreach (var pair in idGroups)
        {
            if (pair.Value.Count >= 3)//���⿡ �߰������� ��ư�� �����߸� �������� �����ǰԸ����
            {
                List<GameObject> targets = pair.Value.GetRange(0, 3); // ó�� 3���� ���׷��̵� ������� ���
                Vector2 spawnPos = targets[0].transform.position;

                // 3. ���� ������Ʈ ����
                foreach (GameObject obj in targets)
                {
                    spawnedObjects.Remove(obj);
                    Destroy(obj);
                }

                // 4. ���� ������Ʈ ���� ���� �߱��� ���̵� 5~10 / ��� 11~14
                int randomIndex = UnityEngine.Random.Range(0, spawner2.prefabsToSpawn.Length);
                GameObject upgradePrefab = spawner2.prefabsToSpawn[randomIndex];
                GameObject upgraded = Instantiate(upgradePrefab, spawnPos, Quaternion.identity);

                RegisterObject(upgraded); // ����Ʈ�� ���� ���

                // 5. ��ư ��Ȱ��ȭ
                upgradeButton.SetActive(false);

                Debug.Log("���׷��̵� �Ϸ�!");
                return; // �� ���� ó���ϰ� ����
            }
        }

        Debug.Log("���׷��̵� ������ �����ϴ� ����� �����ϴ�.");
    }

    public void LvUpSp()
    {
        GameObject obj1 = null, obj2 = null, obj3 = null;

        // �� ID�� �ش��ϴ� ������Ʈ �ϳ��� ã��
        foreach (GameObject obj in spawnedObjects)
        {
            CharacterInstance ci = obj.GetComponent<CharacterInstance>();
            if (ci == null) continue;

            if (ci.characterId == 1 /*ĳ���;��̵�*/&& obj1 == null) obj1 = obj;
            else if (ci.characterId == 2 /*ĳ���;��̵�*/&& obj2 == null) obj2 = obj;
            else if (ci.characterId == 5 /*ĳ���;��̵�*/&& obj3 == null) obj3 = obj;

            if (obj1 && obj2 && obj3) break; // �� ã������ ����
        }

        if (obj1 == null || obj2 == null || obj3 == null)
        {
            Debug.Log("ID 1, 2, 3 ������Ʈ�� ��� �������� �ʽ��ϴ�.");
            return;
        }
        Vector2 s = Vector2.zero;
        // ���� ������Ʈ ����
        spawnedObjects.Remove(obj1); Destroy(obj1);
        spawnedObjects.Remove(obj2); Destroy(obj2);
        spawnedObjects.Remove(obj3); Destroy(obj3);

        GameObject upgraded = Instantiate(Sp1, s, Quaternion.identity);
        RegisterObject(upgraded); // �� ������Ʈ�� ����Ʈ�� ���
            
        // ��ư ����
        upgradeButton.SetActive(false);

        Debug.Log("ID 1, 2, 3 ���� �� ���׷��̵� �Ϸ�!");
    }
}

