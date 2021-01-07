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
                    hit.transform.GetComponent<HealStation>().BuyHeal(gameObject);
                }
            }
            if(hit.transform.tag == "LauncherPart")
            {
                if (Input.GetButtonDown("Use"))
                {
                    missionScript.RLPickUp(hit.transform.GetComponent<LauncherPart>().partNumber, hit.transform.gameObject);
                }
            }
        }
    }
    public void PlayAudioSource(AudioSource source)
    {
        source.volume = Random.Range(sounds.SoundVolume - .1f, sounds.SoundVolume + .1f);
        source.pitch = Random.Range(1 - .1f, 1 + .1f);
        source.Play();
    }
}
