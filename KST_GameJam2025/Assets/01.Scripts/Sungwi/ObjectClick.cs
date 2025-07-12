using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TMP_Text ��� ��

public class ObjectClick : MonoBehaviour
{

    public NoteManager notemanager;
    public GameObject[] characterPanels;
    public GameObject[] characterObjects;

    public GameObject selectedObject;  // ���õ� ������Ʈ

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log("Ŭ���Ǿ����ϴ�!");
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

        Debug.Log($"ĳ���� {index + 1} Ŭ���� �� �ش� �г� Ȱ��ȭ");
    }

    public void DestroySelectedObject()
    {
        if (selectedObject != null)
        {
            Debug.Log($"������: {selectedObject.name}");
            Destroy(selectedObject);
            selectedObject = null;
        }
        else
        {
            Debug.Log("������ ������Ʈ�� ���õ��� �ʾҽ��ϴ�.");
        }
    }
}
