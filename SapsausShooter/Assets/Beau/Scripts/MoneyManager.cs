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
