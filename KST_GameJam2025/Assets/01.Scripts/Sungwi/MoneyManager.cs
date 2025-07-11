using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public Text moneyText;        // Legacy UI Text
    public int currentMoney = 100000;

    void Start()
    {
        UpdateMoneyText();
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        UpdateMoneyText();
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
            Debug.Log("�ܾ� ����!");
            return false;
        }
    }

    private void UpdateMoneyText()
    {
        if (moneyText != null)
            moneyText.text = "���� �ݾ�: " + currentMoney.ToString("N0") + "��";
    }
}
