using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public TMP_Text moneyText;     
    public TMP_Text diaText;
    
    public int currentMoney = 0;
    public int defaultMoney = 10;
    
    public int currentDia = 0;
    public int defaultDia = 0;

    void Start()
    {
        currentMoney = defaultMoney;
        UpdateMoneyText();
        UpdateDiaText();
    }

    private void Update()
    {
        DebugPlusMoney();
        DebugPlusDia();
    }

    public void DebugPlusMoney()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            AddMoney(10);
        }
    }
    public void DebugPlusDia()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            AddDia(10);
        }
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        UpdateMoneyText();
    }
    
    public void AddDia(int amount)
    {
        currentDia += amount;
        UpdateDiaText();
    }

    public bool SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            UpdateMoneyText();
            return true;
        }
        else
        {
            Debug.Log("잔액 부족!");
            return false;
        }
    }
    
    public bool SpendDia(int amount)
    {
        if (currentDia >= amount)
        {
            currentDia -= amount;
            UpdateDiaText();
            return true;
        }
        else
        {
            Debug.Log("잔액 부족!");
            return false;
        }
    }

    private void UpdateMoneyText()
    {
        if (moneyText != null)
            moneyText.text = "현재 금액: " + currentMoney.ToString("N0") + "원";
    }
    
    private void UpdateDiaText()
    {
        if (diaText != null)
            diaText.text = "현재 금액: " + currentDia.ToString("N0") + "원";
    }
}
