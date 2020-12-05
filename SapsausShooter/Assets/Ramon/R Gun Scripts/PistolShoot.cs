using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShoot : ShootAttack
{
    // Start is called before the first frame update
    void Start()
    {

    }

    void OnEnable()
    {
        //pistolAnimation.SetBool("Reloading", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (weapon.weaponPrefab.GetComponent<GunScript>().weapon.gunType == "Pistol")
        {
            if (isReloading)
            {
                return;
            }

            if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire && weapon != null)
            {
                if (currentSlot.ammoInMag <= 0 || weaponWheel.activeSelf == true)
                {
                    return;
                }

                nextTimeToFire = Time.time + 1f / weapon.weaponPrefab.GetComponent<GunScript>().weapon.fireRate;

                ShootPistol();
            }

            if (Input.GetButtonDown("Reload") && currentSlot.ammoInMag < weapon.weaponPrefab.GetComponent<GunScript>().weapon.magCount)
            {
                if (ammoScript.pistolAmmo <= 0)
                {
                    print("NoAmmo");
                    return;
                }
                else
                {
                    ammoScript.pistolAmmo += currentSlot.ammoInMag;
                    if (ammoScript.pistolAmmo >= weapon.weaponPrefab.GetComponent<GunScript>().weapon.magCount)
                    {
                        addAmmo = weapon.weaponPrefab.GetComponent<GunScript>().weapon.magCount;
                    }
                    else
                    {
                        addAmmo = ammoScript.pistolAmmo;
                    }
                    ammoScript.pistolAmmo -= addAmmo;
                    ammoScript.UpdatePistolAmmoLeft();
                }
                StartCoroutine(Reload());
            }

        }
    }

    void ShootPistol()
    {
        //pistolAnimation.SetBool("Shoot", true);

        currentSlot.ammoInMag--;
        ammoScript.UpdateAmmo(currentSlot.ammoInMag);

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

        //pistolAnimation.SetBool("Shoot", false);
    }
}
