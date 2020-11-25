using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoCounter : MonoBehaviour
{
    public float pistolAmmo, shotgunAmmo, sniperAmmo, launcherAmmo, specialAmmo;
    public TextMeshProUGUI ammoLeftText, magText, ammoInInvText;
    public GameObject weaponSelect;

    private void Update()
    {
        if(weaponSelect.activeSelf == true)
        {
            ammoInInvText.text = pistolAmmo + "<br>" + shotgunAmmo + "<br>" + sniperAmmo + "<br>" + launcherAmmo + "<br>" + specialAmmo;
        }
    }
    public void UpdatePistolAmmoLeft()
    {
        ammoLeftText.text = pistolAmmo.ToString();
    }
    public void UpdateAmmo(float ammo)
    {
        magText.text = ammo.ToString();
    }
}
