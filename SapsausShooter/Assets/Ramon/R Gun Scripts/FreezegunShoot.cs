using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezegunShoot : ShootAttack
{
    public FreezeHitBox freezeBox;
    void Start()
    {
        //freezeSpeed = spAnimator.GetFloat("speed");
    }

    public override void Update()
    {
        if (weapon.weaponPrefab.GetComponent<GunScript>().weapon.gunType == "Freezegun")
        {
            base.Update();
        }
    }

    public override void FireWeapon()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && weapon != null)
        {
            FireRateAndSwitch();

            ShootWeapon();

            SoundWave();
        }
    }
    public override void ReloadWeapon()
    {
        //if (weapon.weaponPrefab.GetComponent<WeaponScript>().weapon.gunType == "Freezegun")
        //{
            //if (ammoScript.freezegunAmmo <= 0)
            //{
                //print("NoAmmo");
                //return;
            //}
            //else
            //{
                //ammoScript.freezegunAmmo += currentSlot.ammoInMag;
                //if (ammoScript.freezegunAmmo >= weapon.weaponPrefab.GetComponent<WeaponScript>().weapon.magCount)
                //{
                    //addAmmo = weapon.weaponPrefab.GetComponent<WeaponScript>().weapon.magCount;
                //}
                //else
                //{
                    //addAmmo = ammoScript.freezegunAmmo;
                //}
            //ammoScript.freezegunAmmo -= addAmmo;
            //ammoScript.UpdateFreezegunAmmoLeft();
            //}
        //}
    }
    public override void ShootWeapon()
    {
        //freezegunAnimation.SetBool("Shoot", true);

        currentSlot.ammoInMag--;
        ammoScript.UpdateAmmo(currentSlot.ammoInMag);

        //weapon.muzzleFlash.Play();


        hitMarkerObj = whiteHitMarkerObj;
        StopCoroutine(coroutine);
        coroutine = HitMarker();
        StartCoroutine(coroutine);

        //GameObject impactGO = Instantiate(weapon.impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        //Destroy(impactGO, 2f);


        //freezegunAnimation.SetBool("Shoot", false);
    }
}
