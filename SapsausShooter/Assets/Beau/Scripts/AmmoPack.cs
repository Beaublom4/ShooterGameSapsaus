﻿using UnityEngine;

public class AmmoPack : MonoBehaviour
{
    public string ammoType;
    public int minAmmo, maxAmmo;
    public AmmoCounter ammoScript;
    public WeaponSelector selectScript;

    private void Start()
    {
        ammoScript = GameObject.Find("WeaponManager").GetComponent<AmmoCounter>();
        selectScript = GameObject.Find("WeaponManager").GetComponent<WeaponSelector>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUpCol")
        {
            if (ammoType == "Pistol")
            {
                int ammo = Random.Range(minAmmo, maxAmmo);
                ammoScript.pistolAmmo += ammo;
            }
            else if (ammoType == "Sniper")
            {
                int ammo = Random.Range(minAmmo, maxAmmo);
                ammoScript.sniperAmmo += ammo;
            }
            else if ( ammoType == "Shotgun")
            {
                int ammo = Random.Range(minAmmo, maxAmmo);
                ammoScript.shotgunAmmo += ammo;
            }

            if (selectScript.selectedSlotScript.weapon.gunType == "Pistol")
            {
                ammoScript.UpdatePistolAmmoLeft();
            }
            else if(selectScript.selectedSlotScript.weapon.gunType == "Sniper")
            {
                ammoScript.UpdateSniperAmmoLeft();
            }
            else if (selectScript.selectedSlotScript.weapon.gunType == "Shotgun")
            {
                ammoScript.UpdateShotgunAmmoLeft();
            }

            Destroy(gameObject);
        }
    }
}
