using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    [Header("패널")]
    public GameObject settingPanel;
    public GameObject roulettePanel;
    public GameObject statusPanel;

    [Header("버튼 오브젝트 (GameObject 자체를 넣으세요!)")]
    public GameObject settingButtonObj;
    public GameObject rouletteButtonObj;
    public GameObject statusButtonObj;

    private Vector3 settingDefaultPos;
    private Vector3 rouletteDefaultPos;
    private Vector3 statusDefaultPos;

    [Header("이동 거리")]
    public float offsetX = -30f;

    void Start()
    {
        // 시작 시 버튼의 원래 로컬 위치 저장
        settingDefaultPos = settingButtonObj.transform.localPosition;
        rouletteDefaultPos = rouletteButtonObj.transform.localPosition;
        statusDefaultPos = statusButtonObj.transform.localPosition;
        OnClickRoulette();
    }

    public void OnClickSetting()
    {
        settingPanel.SetActive(true);
        roulettePanel.SetActive(false);
        statusPanel.SetActive(false);

        MoveButton(settingButtonObj, settingDefaultPos);
        ResetButton(rouletteButtonObj, rouletteDefaultPos);
        ResetButton(statusButtonObj, statusDefaultPos);
    }

    public void OnClickRoulette()
    {
        settingPanel.SetActive(false);
        roulettePanel.SetActive(true);
        statusPanel.SetActive(false);

        MoveButton(rouletteButtonObj, rouletteDefaultPos);
        ResetButton(settingButtonObj, settingDefaultPos);
        ResetButton(statusButtonObj, statusDefaultPos);
    }

    public void OnClickStatus()
    {
        settingPanel.SetActive(false);
        roulettePanel.SetActive(false);
        statusPanel.SetActive(true);

        MoveButton(statusButtonObj, statusDefaultPos);
        ResetButton(settingButtonObj, settingDefaultPos);
        ResetButton(rouletteButtonObj, rouletteDefaultPos);
    }

    private void MoveButton(GameObject buttonObj, Vector3 defaultPos)
    {
        buttonObj.transform.localPosition = defaultPos + new Vector3(offsetX, 0, 0);
    }

    private void ResetButton(GameObject buttonObj, Vector3 defaultPos)
    {
        buttonObj.transform.localPosition = defaultPos;
    }
}
