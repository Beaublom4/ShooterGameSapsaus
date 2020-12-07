using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunShoot : ShootAttack
{
    public override void Update()
    {
        if (weapon.weaponPrefab.GetComponent<GunScript>().weapon.gunType == "Shotgun")
        {
            ShotgunScatter();

            base.Update();
        }
    }

    public void ShotgunScatter()
    {
        randomDir = fpsCam.transform.forward;
        randomDir += Random.Range(-scattering, scattering) * transform.right;
    }
    public override void ReloadWeapon()
    {
        if (ammoScript.shotgunAmmo <= 0)
        {
            print("NoAmmo");
            return;
        }
        else
        {
            ammoScript.shotgunAmmo += currentSlot.ammoInMag;
            if (ammoScript.shotgunAmmo >= weapon.weaponPrefab.GetComponent<GunScript>().weapon.magCount)
            {
                addAmmo = weapon.weaponPrefab.GetComponent<GunScript>().weapon.magCount;
            }
            else
            {
                addAmmo = ammoScript.shotgunAmmo;
            }
            ammoScript.shotgunAmmo -= addAmmo;
            ammoScript.UpdateShotgunAmmoLeft();
        }
    }

    public override void ShootWeapon()
    {
        //shotgunAnimation.SetBool("Shoot", true);

        currentSlot.ammoInMag--;
        ammoScript.UpdateAmmo(currentSlot.ammoInMag);

        for (int i = 0; i < Mathf.Max(1, shotPellets); i++)
        {
            //weapon.muzzleFlash.Play();
            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, 1000, canHit, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.tag == "Enemy")
                {
                    if (hit.collider.GetComponent<BodyHit>())
                    {
                        hit.collider.GetComponent<BodyHit>().HitPart(weapon, hit.point);

                        if (hit.collider.GetComponent<BodyHit>().bodyType == 1)
                        {
                            hitMarkerObj = redHitMarkerObj;
                        }
                        else
                            hitMarkerObj = whiteHitMarkerObj;
                        StopCoroutine(coroutine);
                        coroutine = HitMarker();
                        StartCoroutine(coroutine);
                    }
                }
                //GameObject impactGO = Instantiate(weapon.impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                //Destroy(impactGO, 2f);
            }
        }
        //shotgunAnimation.SetBool("Shoot", false);
    }
}
