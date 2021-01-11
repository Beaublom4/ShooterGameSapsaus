using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoCounter : MonoBehaviour
{
    public float pistolAmmo, shotgunAmmo, sniperAmmo, launcherAmmo, specialAmmo;
    public TextMeshProUGUI ammoLeftText, magText, ammoInInvText;
    public GameObject weaponSelect;
    public void UpdatePistolAmmoLeft()
    {
        ammoLeftText.text = pistolAmmo.ToString("F0");
    }
    public void UpdateSniperAmmoLeft()
    {
        ammoLeftText.text = sniperAmmo.ToString("F0");
    }
    public void UpdateShotgunAmmoLeft()
    {
        ammoLeftText.text = shotgunAmmo.ToString("F0");
    }
    public void UpdateLauncherAmmoLeft()
    {
        ammoLeftText.text = launcherAmmo.ToString("F0");
    }
    public void UpdateSpecialAmmoLeft()
    {
        ammoLeftText.text = specialAmmo.ToString("F0");
    }
    public void UpdateAmmo(float ammo)
    {
        magText.text = ammo.ToString("F0");
    }
    public void UpdateMeleeAmmo(float ammo)
    {
        ammoLeftText.text = "1";
        magText.text = ammo.ToString("F0");
    }
}
