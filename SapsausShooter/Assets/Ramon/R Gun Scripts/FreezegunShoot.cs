using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezegunShoot : ShootAttack
{
    // Start is called before the first frame update
    void Start()
    {
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = freezegunCollider;

        //freezeSpeed = spAnimator.GetFloat("speed");
    }

    // Update is called once per frame
    void Update()
    {
        if (weapon.weaponPrefab.GetComponent<GunScript>().weapon.gunType == "Freezegun")
        {
            if (isReloading)
            {
                return;
            }

            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && weapon != null)
            {
                if (currentSlot.ammoInMag <= 0)
                {
                    return;
                }

                nextTimeToFire = Time.time + 1f / weapon.weaponPrefab.GetComponent<GunScript>().weapon.fireRate;

                ShootFreezegun();
            }

            if (Input.GetButtonDown("Reload") && currentSlot.ammoInMag < weapon.weaponPrefab.GetComponent<GunScript>().weapon.magCount)
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
                StartCoroutine(Reload());
            }
        }
    }
    void ShootFreezegun()
    {
        //freezegunAnimation.SetBool("Shoot", true);

        currentSlot.ammoInMag--;
        ammoScript.UpdateAmmo(currentSlot.ammoInMag);

        //weapon.muzzleFlash.Play();
        if (freezegunCollider)
        {
            hitMarkerObj = whiteHitMarkerObj;
            StopCoroutine(coroutine);
            coroutine = HitMarker();
            StartCoroutine(coroutine);
        }
        //GameObject impactGO = Instantiate(weapon.impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        //Destroy(impactGO, 2f);


        //freezegunAnimation.SetBool("Shoot", false);
    }
}
