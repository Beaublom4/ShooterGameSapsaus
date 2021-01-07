using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsePlayer : MonoBehaviour
{
    [System.Serializable]
    public class Sounds
    {
        public float SoundVolume = .5f;
        public AudioSource itemPickUp, Use;
    }
    public GameObject fpsCam;
    public MissionManager missionScript;
    public float useRange;
    RaycastHit hit;
    public Sounds sounds;
    public GameObject pickUpPanel, healPanel, shopPanel;
    private void Update()
    {
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, useRange))
        {
            if (Input.GetButtonDown("Use"))
            {
                if (hit.transform.GetComponent<GarageDoor>())
                {
                    PlayAudioSource(sounds.Use);
                    hit.transform.GetComponent<GarageDoor>().OpenGarageDoor();
                }
            }
            if (hit.transform.tag == "ShopItem")
            {
                shopPanel.SetActive(true);
                if (hit.transform.GetComponent<ShopItem>().canvas.activeSelf == false)
                {
                    hit.transform.GetComponent<ShopItem>().ActivateScript();
                }
                if (Input.GetButtonDown("Use"))
                {
                    hit.transform.GetComponent<ShopItem>().BuyItem(gameObject);
                }
            }
            else if (shopPanel.activeSelf == true)
            {
                shopPanel.SetActive(false);

            }
            if (hit.transform.tag == "HealStation")
            {
                healPanel.SetActive(true);
                if (hit.transform.GetComponent<HealStation>().infoPanel.activeSelf == false)
                {
                    hit.transform.GetComponent<HealStation>().ShowPrice();
                }
                if (Input.GetButtonDown("Use"))
                {
                    hit.transform.GetComponent<HealStation>().BuyHeal(gameObject);
                }

            }
            else if (healPanel.activeSelf == true)
            {
                healPanel.SetActive(false);
            }
            if (hit.transform.tag == "LauncherPart")
            {
                pickUpPanel.SetActive(true);
                if (Input.GetButtonDown("Use"))
                {
                    missionScript.RLPickUp(hit.transform.GetComponent<LauncherPart>().partNumber, hit.transform.gameObject);
                }
            }
            else if (shopPanel.activeSelf == true)
            {
                pickUpPanel.SetActive(false);

            }
        }
        else
        {
            healPanel.SetActive(false);
            shopPanel.SetActive(false);
            pickUpPanel.SetActive(false);
        }
    }
    public void PlayAudioSource(AudioSource source)
    {
        source.volume = Random.Range(sounds.SoundVolume - .1f, sounds.SoundVolume + .1f);
        source.pitch = Random.Range(1 - .1f, 1 + .1f);
        source.Play();
    }
}
