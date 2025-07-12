using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.U2D.Animation;
public class ObjectManager : MonoBehaviour
{
    public static ObjectManager Instance; // 싱글톤 패턴을 위한 static 인스턴스 변수
    public Spawner spawner; // 오브젝트를 생성하는 스크립트의 참조
    public Spawner2 spawner2; // 상위계체 생성코드
    public GameObject upgradeButton; // 업그레이드 버튼 오브젝트 (비활성화/활성화용)
    public GameObject upgradeButtonSp; //상위업글 버튼
    public GameObject Sp1;
    public GameObject Sp2;

    public List<GameObject> spawnedObjects = new List<GameObject>(); // 생성된 오브젝트들을 저장하는 리스트

    private void Awake()
    {
        if (Instance == null)         // Instance가 비어있다면
            Instance = this;          // 현재 인스턴스를 싱글톤으로 설정
        else
            Destroy(gameObject);      // 이미 다른 인스턴스가 있다면 중복 방지를 위해 제거
    }

    public void RegisterObject(GameObject obj)
    {
        spawnedObjects.Add(obj); // 생성된 오브젝트를 리스트에 등록

    }

    public void UnregisterObject(GameObject obj)
    {
        spawnedObjects.Remove(obj); // 오브젝트를 리스트에서 제거

    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 스페이스바를 누르면
        {
            spawner.SpawnObject(new Vector2(0, 0)); // (0,0) 위치에 오브젝트를 생성
            //격자칸 맞춰서 랜덤생성
            CheckUpgradeAvailable();
            CheckUpgradeAvailableSp();
        }



    }

    public void CheckUpgradeAvailable()
    {
        Dictionary<int, int> countById = new Dictionary<int, int>(); // 캐릭터 ID별로 개수를 세는 딕셔너리 생성

        foreach (GameObject obj in spawnedObjects) // 생성된 오브젝트를 모두 순회
        {
            CharacterInstance ci = obj.GetComponent<CharacterInstance>(); // CharacterInstance 컴포넌트 가져오기
            if (ci == null) continue; // 없으면 무시

            if (!countById.ContainsKey(ci.characterId)) // 해당 ID가 없으면
                countById[ci.characterId] = 1;           // 1로 초기화

            else
                countById[ci.characterId]++;             // 이미 있으면 +1
            //id 값이 3이 되면 0으로 초기화
        }

        foreach (var pair in countById) // 딕셔너리의 모든 값 확인
        {
            if (pair.Value >= 3) // 같은 ID의 캐릭터가 3개 이상이면
            {
                upgradeButton.SetActive(true); // 업그레이드 버튼 활성화
                return; // 조건 만족하면 바로 종료
                //버튼을 누른다면 비활성화
                //이미지말고 컴포넌트 활성화 비활성화
                //같은 캐릭터 위치에 상위등급개체 젠
            }
        }



        upgradeButton.SetActive(false); // 조건 만족하는 캐릭터가 없으면 버튼 비활성화
    }

    public void CheckUpgradeAvailableSp()
    {
        // ID별로 존재 여부 확인
        bool has1 = false;
        bool has2 = false;
        bool has3 = false;

        foreach (GameObject obj in spawnedObjects)
        {
            CharacterInstance ci = obj.GetComponent<CharacterInstance>();
            if (ci == null) continue;

            if (ci.characterId == 1/*조합식 짜기 캐릭터아이디*/) has1 = true;
            if (ci.characterId == 2/*캐릭터아이디*/) has2 = true;
            if (ci.characterId == 5/*캐릭터아이디*/) has3 = true;
        }

        if (has1 && has2 && has3)
            upgradeButtonSp.SetActive(true);
        else
            upgradeButtonSp.SetActive(false);
    }

    public void LvUp1()
    {
        Dictionary<int, List<GameObject>> idGroups = new Dictionary<int, List<GameObject>>();

        // 1. 같은 ID끼리 그룹 만들기
        foreach (GameObject obj in spawnedObjects)
        {
            CharacterInstance ci = obj.GetComponent<CharacterInstance>();
            if (ci == null) continue;

            int id = ci.characterId;

            if (!idGroups.ContainsKey(id))
                idGroups[id] = new List<GameObject>();

            idGroups[id].Add(obj);
        }

        // 2. 업그레이드 대상 찾기 (3개 이상인 그룹)
        foreach (var pair in idGroups)
        {
            if (pair.Value.Count >= 3)//여기에 추가적으로 버튼을 눌러야만 없어지고 생성되게만들기
            {
                List<GameObject> targets = pair.Value.GetRange(0, 3); // 처음 3개만 업그레이드 대상으로 사용
                Vector2 spawnPos = targets[0].transform.position;

                // 3. 기존 오브젝트 제거
                foreach (GameObject obj in targets)
                {
                    spawnedObjects.Remove(obj);
                    Destroy(obj);
                }

                // 4. 상위 오브젝트 랜덤 생성 중급은 아이디값 5~10 / 상급 11~14
                int randomIndex = UnityEngine.Random.Range(0, spawner2.prefabsToSpawn.Length);
                GameObject upgradePrefab = spawner2.prefabsToSpawn[randomIndex];
                GameObject upgraded = Instantiate(upgradePrefab, spawnPos, Quaternion.identity);

                RegisterObject(upgraded); // 리스트에 새로 등록

                // 5. 버튼 비활성화
                upgradeButton.SetActive(false);

                Debug.Log("업그레이드 완료!");
                return; // 한 번만 처리하고 끝냄
            }
        }

        Debug.Log("업그레이드 조건을 만족하는 대상이 없습니다.");
    }

    public void LvUpSp()
    {
        GameObject obj1 = null, obj2 = null, obj3 = null;

        // 각 ID에 해당하는 오브젝트 하나씩 찾기
        foreach (GameObject obj in spawnedObjects)
        {
            CharacterInstance ci = obj.GetComponent<CharacterInstance>();
            if (ci == null) continue;

            if (ci.characterId == 1 /*캐릭터아이디*/&& obj1 == null) obj1 = obj;
            else if (ci.characterId == 2 /*캐릭터아이디*/&& obj2 == null) obj2 = obj;
            else if (ci.characterId == 5 /*캐릭터아이디*/&& obj3 == null) obj3 = obj;

            if (obj1 && obj2 && obj3) break; // 다 찾았으면 종료
        }

        if (obj1 == null || obj2 == null || obj3 == null)
        {
            Debug.Log("ID 1, 2, 3 오브젝트가 모두 존재하지 않습니다.");
            return;
        }
        Vector2 s = Vector2.zero;
        // 기존 오브젝트 제거
        spawnedObjects.Remove(obj1); Destroy(obj1);
        spawnedObjects.Remove(obj2); Destroy(obj2);
        spawnedObjects.Remove(obj3); Destroy(obj3);

        GameObject upgraded = Instantiate(Sp1, s, Quaternion.identity);
        RegisterObject(upgraded); // 새 오브젝트도 리스트에 등록
            
        // 버튼 끄기
        upgradeButton.SetActive(false);

        Debug.Log("ID 1, 2, 3 조합 → 업그레이드 완료!");
    }
}

