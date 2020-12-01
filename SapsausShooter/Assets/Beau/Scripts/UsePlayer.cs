using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsePlayer : MonoBehaviour
{
    public GameObject fpsCam;
    public float useRange;
    RaycastHit hit;
    private void Update()
    {
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, useRange))
        {
            if (Input.GetButtonDown("Use"))
            {
                if (hit.transform.GetComponent<GarageDoor>())
                {
                    hit.transform.GetComponent<GarageDoor>().OpenGarageDoor();
                }
            }
            if(hit.transform.tag == "ShopItem")
            {
                if (hit.transform.GetComponent<ShopItem>().canvas.activeSelf == false)
                {
                    hit.transform.GetComponent<ShopItem>().ActivateScript();
                }
                if (Input.GetButtonDown("Use"))
                {
                    hit.transform.GetComponent<ShopItem>().BuyItem(gameObject);
                }
            }
            if(hit.transform.tag == "HealStation")
            {
                if (hit.transform.GetComponent<HealStation>().infoPanel.activeSelf == false)
                {
                    hit.transform.GetComponent<HealStation>().ShowPrice();
                }
                if (Input.GetButtonDown("Use"))
                {
                    hit.transform.GetComponent<HealStation>().BuyHeal();
                }
            }
        }
    }
}
