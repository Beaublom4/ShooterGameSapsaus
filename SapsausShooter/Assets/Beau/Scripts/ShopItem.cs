using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopItem : MonoBehaviour
{
    public string nameObj;
    public int price;

    public GameObject canvas;

    private void Start()
    {
        canvas.GetComponentInChildren<TextMeshProUGUI>().text = nameObj + "<br> Price: " + price;
    }
    public void ActivateScript()
    {
        canvas.SetActive(true);
        Invoke("UnActivate", 2);
    }
    void UnActivate()
    {
        canvas.SetActive(false);
    }
    public void BuyItem(GameObject player)
    {
        MoneyManager moneyManagerScript = player.GetComponent<MoneyManager>();
        if (moneyManagerScript.money >= price) 
        {
            moneyManagerScript.DecreaseMoney(price);
            Destroy(gameObject);
        }
        else
        {
            print("Not enough money");
        }
    }
}
