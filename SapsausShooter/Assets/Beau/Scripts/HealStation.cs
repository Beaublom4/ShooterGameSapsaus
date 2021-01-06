using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealStation : MonoBehaviour
{
    public HealthManager healthScript;
    public MoneyManager moneyScript;

    public float priceHeal;
    float wantedPrice;
    public float increaseNum;

    public TextMeshProUGUI text;
    public GameObject infoPanel;
    public void ShowPrice()
    {
        if (healthScript.health == healthScript.healthSlider.maxValue)
        {
            text.text = "Health Station";
        }
        else
        {
            wantedPrice = (healthScript.healthSlider.maxValue - healthScript.health) * priceHeal;
            text.text = "Heal Station <br> Price: " + wantedPrice;
        }
        infoPanel.SetActive(true);
        Invoke("hideObj", 2);
    }
    public void BuyHeal(GameObject player)
    {
        if(moneyScript.money > priceHeal)
        {
            moneyScript.money -= (int) wantedPrice;
            priceHeal *= increaseNum;

            healthScript.health += healthScript.healthSlider.maxValue - healthScript.health;
            healthScript.UpdateNumber();
            player.GetComponentInChildren<Animator>().SetTrigger("Heal");
            ShowPrice();
            return;
        }
        print("Not enough money");
    }
    void hideObj()
    {
        infoPanel.SetActive(false);
    }
}
