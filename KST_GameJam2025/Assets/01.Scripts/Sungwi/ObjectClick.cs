using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TMP_Text 사용 시

public class ObjectClick : MonoBehaviour
{

    public NoteManager notemanager;
    public GameObject[] characterPanels;
    public GameObject[] characterObjects;

    public GameObject selectedObject;  // 선택된 오브젝트

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log("클릭되었습니다!");
                notemanager.OnClickStatus();

                for (int i = 0; i < characterObjects.Length; i++)
                {
                    if (hit.collider.gameObject == characterObjects[i])
                    {
                        selectedObject = hit.collider.gameObject;
                        ActivatePanel(i);
                        break;
                    }
                }
            }
        }
    }

    void ActivatePanel(int index)
    {
        for (int i = 0; i < characterPanels.Length; i++)
        {
            characterPanels[i].SetActive(i == index);
        }

        Debug.Log($"캐릭터 {index + 1} 클릭됨 → 해당 패널 활성화");
    }

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
            Debug.Log("삭제할 오브젝트가 선택되지 않았습니다.");
        }
    }
}
