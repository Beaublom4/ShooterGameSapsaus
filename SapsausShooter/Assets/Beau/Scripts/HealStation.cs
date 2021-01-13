using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealStation : MonoBehaviour
{
    public HealthManager healthScript;
    public MoneyManager moneyScript;

    public float priceHeal;
    public float wantedPrice;
    public float increaseNum;

    public TextMeshProUGUI text;
    public GameObject infoPanel;
    public GameObject blikkkkieeee;
    private void Start()
    {
        GameObject deNigga = GameObject.FindGameObjectWithTag("Player");
        healthScript = deNigga.GetComponent<HealthManager>();
        moneyScript = deNigga.GetComponent<MoneyManager>();
    }
    public void ShowPrice()
    {
        if (healthScript.health < healthScript.healthSlider.maxValue)
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
        if(moneyScript.money > wantedPrice && healthScript.health < healthScript.healthSlider.maxValue)
        {
            moneyScript.DecreaseMoney((int)wantedPrice);
            priceHeal *= increaseNum;

            healthScript.health += healthScript.healthSlider.maxValue - healthScript.health;
            healthScript.UpdateNumber();
            if (player.GetComponent<ShootAttack>().currentSlot != null)
            {
                if (player.GetComponent<ShootAttack>().currentSlot.gunWeapon != null)
                {
                    print("Heal2");
                    StartCoroutine(Drink(player));
                }
            }
            else
            {
                print("Heal2");
                StartCoroutine(Drink(player));
            }
            GetComponent<AudioSource>().Play();
            ShowPrice();
            return;
        }
        print("Not enough money");
    }
    IEnumerator Drink(GameObject nig)
    {
        yield return new WaitForSeconds(3);
        nig.GetComponentInChildren<Animator>().SetTrigger("Heal");
        GameObject g = Instantiate(blikkkkieeee, nig.GetComponent<ShootAttack>().blikkieLoc.transform.position, nig.GetComponent<ShootAttack>().blikkieLoc.transform.rotation, nig.GetComponent<ShootAttack>().blikkieLoc.transform);
        Destroy(g, .7f);
    }
    void hideObj()
    {
        infoPanel.SetActive(false);
    }
}
