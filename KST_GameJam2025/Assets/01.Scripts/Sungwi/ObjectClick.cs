using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TMP_Text ��� ��

public class ObjectClick : MonoBehaviour
{
  public NoteManager notemanager; // 클릭 시 상태를 띄워줄 매니저

    public GameObject[] characterPanels;     // 영웅에 대응되는 정보 패널들
    public GameObject[] characterPrefabs;    // 영웅 프리팹 (캐릭터 종류들)

    public GameObject selectedObject;        // 현재 선택된 오브젝트

    void Update()
    {
        // 마우스 왼쪽 클릭 감지
        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 위치를 월드 좌표로 변환
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 해당 위치에 2D 레이캐스트
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log("오브젝트가 클릭되었습니다!");
                notemanager.OnClickStatus(); // 상태창 열기

                // 클릭한 오브젝트가 HeroIdentity를 가지고 있는지 확인
                HeroIdentity identity = hit.collider.GetComponent<HeroIdentity>();

                if (identity != null)
                {
                    selectedObject = hit.collider.gameObject; // 현재 선택된 오브젝트 저장
                    ActivatePanel(identity.prefabIndex);      // 해당하는 정보 패널 열기
                }
                else
                {
                    Debug.Log("HeroIdentity 정보가 없는 오브젝트입니다.");
                }
            }
        }
    }

    // index에 해당하는 패널만 활성화하고 나머지는 비활성화
    void ActivatePanel(int index)
    {
        for (int i = 0; i < characterPanels.Length; i++)
        {
            characterPanels[i].SetActive(i == index);
        }

        Debug.Log($"영웅 {index + 1} 클릭됨 → 정보 패널 {index}번 활성화");
    }

    // 선택된 오브젝트 삭제 (예: 팔기, 제거 등)
    public void DestroySelectedObject()
    {
        if (selectedObject != null)
        {
            Debug.Log($"삭제됨: {selectedObject.name}");
            Destroy(selectedObject);
            selectedObject = null;
        }
        else
        {
            Debug.Log("선택된 오브젝트가 없습니다.");
        }
    }

    // 외부에서 프리팹을 스폰할 때 사용하는 함수
    public GameObject SpawnHero(int index, Vector2 spawnPos)
    {
        if (index < 0 || index >= characterPrefabs.Length)
        {
            Debug.LogWarning("유효하지 않은 프리팹 인덱스입니다.");
            return null;
        }

        // 프리팹 생성
        GameObject obj = Instantiate(characterPrefabs[index], spawnPos, Quaternion.identity);

        // 자신이 몇 번째 프리팹에서 왔는지 기억
        obj.AddComponent<HeroIdentity>().prefabIndex = index;

        return obj;
    }
}
