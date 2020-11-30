using UnityEngine;
using System.Collections;
using TMPro;

public class ShootAttack : MonoBehaviour
{
    public Weapon weapon;
    public Camera fpsCam;
    public Animator pistolAnimation;

    public LayerMask canHit;

    public Vector3 randomDir;

    private float nextTimeToFire = 0f;
    public float scattering = 1;

    public int shotPellets = 8;

    public bool canShoot;
    public bool isReloading = false;

    float addAmmo;
    public Slot currentSlot;
    public AmmoCounter ammoScript;

    public GameObject whiteHitMarkerObj, redHitMarkerObj, hitMarkerObj;
    public float displayTimeHitMarker;
    IEnumerator coroutine;
    void Start()
    {
        //currentMagCount = weapon.magCount;
        coroutine = HitMarker();
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

            if (weapon.weaponPrefab.GetComponent<WeaponScript>().weapon.gunType == "Pistol")
            {
                ShootPistol();
            }

            if (weapon.weaponPrefab.GetComponent<WeaponScript>().weapon.gunType == "Sniper")
            {
                ShootSniper();
            }

            if (weapon.weaponPrefab.GetComponent<WeaponScript>().weapon.gunType == "Shotgun")
            {
                randomDir = fpsCam.transform.forward;
                randomDir += Random.Range(-scattering, scattering) * transform.right;
                ShootShotgun();
            }
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && weapon != null)
        {
            if (currentSlot.ammoInMag <= 0)
            {
                return;
            }

            nextTimeToFire = Time.time + 1f / weapon.weaponPrefab.GetComponent<WeaponScript>().weapon.fireRate;

            if (weapon.weaponPrefab.GetComponent<WeaponScript>().weapon.gunType == "Freezegun")
            {
                ShootFreezegun();
            }
        }

        if (Input.GetButtonDown("Reload") && currentSlot.ammoInMag < weapon.weaponPrefab.GetComponent<WeaponScript>().weapon.magCount)
        {
            if (weapon.weaponPrefab.GetComponent<WeaponScript>().weapon.gunType == "Pistol")
            {
                if (ammoScript.pistolAmmo <= 0)
                {
                    print("NoAmmo");
                    return;
                }
                else
                {
                    ammoScript.pistolAmmo += currentSlot.ammoInMag;
                    if (ammoScript.pistolAmmo >= weapon.weaponPrefab.GetComponent<WeaponScript>().weapon.magCount)
                    {
                        addAmmo = weapon.weaponPrefab.GetComponent<WeaponScript>().weapon.magCount;
                    }
                    else
                    {
                        addAmmo = ammoScript.pistolAmmo;
                    }
                    ammoScript.pistolAmmo -= addAmmo;
                    ammoScript.UpdatePistolAmmoLeft();
                }
            }

            if (weapon.weaponPrefab.GetComponent<WeaponScript>().weapon.gunType == "Sniper")
            {
                if (ammoScript.sniperAmmo <= 0)
                {
                    print("NoAmmo");
                    return;
                }
                else
                {
                    ammoScript.sniperAmmo += currentSlot.ammoInMag;
                    if (ammoScript.sniperAmmo >= weapon.weaponPrefab.GetComponent<WeaponScript>().weapon.magCount)
                    {
                        addAmmo = weapon.weaponPrefab.GetComponent<WeaponScript>().weapon.magCount;
                    }
                    else
                    {
                        addAmmo = ammoScript.sniperAmmo;
                    }
                    ammoScript.sniperAmmo -= addAmmo;
                    ammoScript.UpdateSniperAmmoLeft();
                }
            }

            if (weapon.weaponPrefab.GetComponent<WeaponScript>().weapon.gunType == "Shotgun")
            {
                if (ammoScript.shotgunAmmo <= 0)
                {
                    print("NoAmmo");
                    return;
                }
                else
                {
                    ammoScript.shotgunAmmo += currentSlot.ammoInMag;
                    if (ammoScript.shotgunAmmo >= weapon.weaponPrefab.GetComponent<WeaponScript>().weapon.magCount)
                    {
                        addAmmo = weapon.weaponPrefab.GetComponent<WeaponScript>().weapon.magCount;
                    }
                    else
                    {
                        addAmmo = ammoScript.shotgunAmmo;
                    }
                    ammoScript.shotgunAmmo -= addAmmo;
                    ammoScript.UpdateShotgunAmmoLeft();
                }
            }

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
    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        //pistolAnimation.SetTrigger("Reload");

        yield return new WaitForSeconds(weapon.weaponPrefab.GetComponent<WeaponScript>().weapon.reloadSpeed);

        print("Reloaded");
        currentSlot.ammoInMag = addAmmo;
        ammoScript.UpdateAmmo(currentSlot.ammoInMag);
        isReloading = false;
    }
    IEnumerator HitMarker()
    {
        hitMarkerObj.SetActive(true);
        yield return new WaitForSeconds(displayTimeHitMarker);
        hitMarkerObj.SetActive(false);
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
                    print(hit.collider.GetComponent<BodyHit>().bodyType);
                    if(hit.collider.GetComponent<BodyHit>().bodyType == 1)
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

    void ShootSniper()
    {
        //sniperAnimation.SetBool("Shoot", true);

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

        //sniperAnimation.SetBool("Shoot", false);
    }

    void ShootShotgun()
    {
        //shotgunAnimation.SetBool("Shoot", true);

        for (int i = 0; i < Mathf.Max(1, shotPellets); i++)
        {
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
        }

        //shotgunAnimation.SetBool("Shoot", false);
    }
    void ShootFreezegun()
    {
        //freezegunAnimation.SetBool("Shoot", true);

        currentSlot.ammoInMag--;
        ammoScript.UpdateAmmo(currentSlot.ammoInMag);

        //weapon.muzzleFlash.Play();
    

            //GameObject impactGO = Instantiate(weapon.impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            //Destroy(impactGO, 2f);
        

        //freezegunAnimation.SetBool("Shoot", false);
    }
}