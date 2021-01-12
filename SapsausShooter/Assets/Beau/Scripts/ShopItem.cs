using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShopItem : MonoBehaviour
{
    [System.Serializable]
    public class Sounds
    {
        public float minSoundRange = .2f, maxSoundRange = .2f;
        public AudioSource buyItem, notEnoughMoney;
    }
    public string nameObj;
    public int price;
    public GameObject shopObject;
    public Sounds sounds;
    public Animator shopKeeper;
    public bool isAmmo;
    public int minAmmount, maxAmmount;
    

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
            shopKeeper.SetTrigger("Sold");
            moneyManagerScript.DecreaseMoney(price);
            GameObject item = Instantiate(shopObject, transform.position, transform.rotation, null);
            if(isAmmo == true)
            {
                item.GetComponent<AmmoPack>().minAmmo = minAmmount;
                item.GetComponent<AmmoPack>().maxAmmo = maxAmmount;

            }
            Destroy(gameObject);
        }
        else
        {
            print("Not enough money");
        }
    }
}
