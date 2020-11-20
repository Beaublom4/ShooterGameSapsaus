using UnityEngine;
using System.Collections;

public class ShootAttack : MonoBehaviour
{
    public Weapon weapon;
    public Camera fpsCam;
    public Animator pistolAnimation;

    public LayerMask canHit;

    private float nextTimeToFire = 0f;

    public bool canShoot;
    public bool isReloading = false;

    float addAmmo;
    public Slot currentSlot;
    public AmmoCounter ammoScript;
    void Start()
    {
        //currentMagCount = weapon.magCount;

        canShoot = true;
    }
    void OnEnable()
    {
        isReloading = false;
        //pistolAnimation.SetBool("Reloading", false);
    }
    void Update()
    {
        if (isReloading)
        {
            return;
        }

        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire && weapon != null)
        {
            if (currentSlot.ammoInMag <= 0)
            {
                return;
            }

            nextTimeToFire = Time.time + 1f / weapon.weaponPrefab.GetComponent<WeaponScript>().weapon.fireRate;

            Shoot();
        }
        if (Input.GetButtonDown("Reload") && currentSlot.ammoInMag < weapon.weaponPrefab.GetComponent<WeaponScript>().weapon.magCount)
        {
            if(weapon.weaponPrefab.GetComponent<WeaponScript>().weapon.gunType == "Pistol")
            {
                if (ammoScript.pistolAmmo <= 0)
                {
                    print("NoAmmo");
                    return;
                }
                else
                {
                    ammoScript.pistolAmmo += currentSlot.ammoInMag;
                    if(ammoScript.pistolAmmo >= weapon.weaponPrefab.GetComponent<WeaponScript>().weapon.magCount)
                    {
                        addAmmo = weapon.weaponPrefab.GetComponent<WeaponScript>().weapon.magCount;
                    }
                    else
                    {
                        addAmmo = ammoScript.pistolAmmo;
                    }
                    ammoScript.pistolAmmo -= addAmmo;
                }
            }
            StartCoroutine(Reload());
        }
    }
    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        //pistolAnimation.SetTrigger("Reload");

        yield return new WaitForSeconds(weapon.weaponPrefab.GetComponent<WeaponScript>().weapon.reloadSpeed);

        print("Reloaded");
        currentSlot.ammoInMag = addAmmo;
        isReloading = false;
    }

    void Shoot()
    {
        //pistolAnimation.SetBool("Shoot", true);

        currentSlot.ammoInMag--;

        //weapon.muzzleFlash.Play();
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, 1000, canHit, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.tag == "Enemy")
            {
                if (hit.collider.GetComponent<BodyHit>())
                {
                    hit.collider.GetComponent<BodyHit>().HitPart(weapon);
                }
            }

            //GameObject impactGO = Instantiate(weapon.impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            //Destroy(impactGO, 2f);
        }

        //pistolAnimation.SetBool("Shoot", false);
    }
}