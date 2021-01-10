using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public int money;

    private void Start()
    {
        if (MainMenuManager.devMode == true)
            money = 9999999;
        UpdateMoney();
    }
    public void GetMoney(int getMoney)
    {
        money += getMoney;
        UpdateMoney();
    }
    public void DecreaseMoney(int decreaseMoney)
    {
        money -= decreaseMoney;
        UpdateMoney();
    }
    void UpdateMoney()
    {
        moneyText.text = "$" + money.ToString();
    }
}
