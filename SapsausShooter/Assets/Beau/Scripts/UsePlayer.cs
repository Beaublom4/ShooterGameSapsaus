using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsePlayer : MonoBehaviour
{
    [System.Serializable]
    public class Sounds
    {
        public float SoundVolume = .5f;
        public AudioSource itemPickUp, Use, shock, lever, powerOut;
    }
    public GameObject fpsCam;
    public MissionManager missionScript;
    public float useRange;
    RaycastHit hit;
    public Sounds sounds;
    public GameObject pickUpPanel, healPanel, shopPanel, startPanel, usePanel;

    public VoiceLineCol startLetter, endLetter;
    private void Start()
    {
        startPanel.SetActive(true);
        startLetter.StartSound();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Use") && startPanel.activeSelf==true)
        {
            startPanel.SetActive(false);
            endLetter.StartSound();
        }


        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, useRange))
        {
            if (Input.GetButtonDown("Use"))
            {
                if (hit.collider.GetComponent<GarageDoor>())
                {
                    PlayAudioSource(sounds.Use);
                    hit.collider.GetComponent<GarageDoor>().OpenGarageDoor();
                }
            }
            if (hit.transform.tag == "ShopItem")
            {
                pickUpPanel.SetActive(true);
                if (Input.GetButtonDown("Use"))
                {
                    pickUpPanel.SetActive(false);
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
                    if (hit.transform.GetComponent<LauncherPart>().cantPickUp == true)
                    {
                        GetComponent<HealthManager>().DoDamage(2);
                        sounds.shock.Play();
                    }
                    else
                    {
                        missionScript.RLPickUp(hit.transform.GetComponent<LauncherPart>().partNumber, hit.transform.gameObject);
                    }
                }
            }
            else if (shopPanel.activeSelf == true)
            {
                pickUpPanel.SetActive(false);

            }
            if(hit.transform.tag == "ElectricSwitch" || hit.transform.tag == "ChurchRope")
            {
                usePanel.SetActive(true);
                if (Input.GetButtonDown("Use"))
                {
                    if(hit.transform.tag == "ElectricSwitch")
                    {
                        hit.transform.GetComponent<ElectricSwitch>().UseSwitch();
                        sounds.lever.Play();
                        sounds.powerOut.Play();
                    }
                    else if(hit.transform.tag == "ChurchRope")
                    {
                        hit.transform.GetComponent<ChurchRope>().DoChurchRope();
                    }
                }
            }
            else if (usePanel.activeSelf == true)
            {
                usePanel.SetActive(false);
            }
        }
        else
        {
            healPanel.SetActive(false);
            shopPanel.SetActive(false);
            pickUpPanel.SetActive(false);
            usePanel.SetActive(false);
        }
    }
    public void PlayAudioSource(AudioSource source)
    {
        source.volume = Random.Range(sounds.SoundVolume - .1f, sounds.SoundVolume + .1f);
        source.pitch = Random.Range(1 - .1f, 1 + .1f);
        source.Play();
    }
}
