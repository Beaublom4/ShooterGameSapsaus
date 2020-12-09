using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsePlayer : MonoBehaviour
{
    [System.Serializable]
    public class Sounds
    {
        public float minSoundRange = .2f, maxSoundRange = .2f;
        public AudioSource itemPickUp, Use;
    }
    public GameObject fpsCam;
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
                    hit.transform.GetComponent<HealStation>().BuyHeal();
                }
            }
        }
    }
    public void PlayAudioSource(AudioSource source)
    {
        float standardVolume = source.volume;
        float standardPitch = source.pitch;
        source.volume = Random.Range(standardVolume - sounds.minSoundRange, standardVolume + sounds.maxSoundRange);
        source.pitch = Random.Range(standardPitch - sounds.minSoundRange, standardPitch + sounds.maxSoundRange);
        source.Play();
        source.volume = standardVolume;
        source.pitch = standardPitch;
    }
}
