using UnityEngine;
using System.Collections;
using TMPro;

public class ShootAttack : MonoBehaviour
{
    public GameObject impactEffect, impactEffect1, impactEffect2, impactEffect3;
    public Weapon weapon;
    public Camera fpsCam;
    public Animator zombieAnimator;
    public GameObject areaColParent;
    public GameObject whiteHitMarkerObj, redHitMarkerObj, hitMarkerObj, weaponWheel;

    public FreezeHitBox freezeBox;

    public LayerMask canHit;

    public Vector3 randomDir;

    public float freezeSpeed = 1f;
    public float nextTimeToFire = 0f;
    public float scattering = 1;
    public float timeUntilFrozen = 2f;
    public float timeUntilDefrozen = 5;

    public int shotPellets = 8;

    public bool canShoot;
    public bool isReloading = false;
    public bool beingFrozen;

    public float addAmmo;
    public Slot currentSlot;
    public AmmoCounter ammoScript;

    public float displayTimeHitMarker;
    public IEnumerator coroutine;
    public IEnumerator colCoroutine;
    void Start()
    {
        //currentMagCount = weapon.magCount;
        coroutine = HitMarker();
        canShoot = true;

        //freezeSpeed = spAnimator.GetFloat("speed");
    }
    void OnEnable()
    {
        isReloading = false;
    }
    void Update()
    {
        if (isReloading)
        {
            return;
        }

        FireWeapon();

        if (Input.GetButtonDown("Reload") && currentSlot.ammoInMag < weapon.weaponPrefab.GetComponent<GunScript>().weapon.magCount)
        {
            ReloadWeapon();

            StartCoroutine(Reload());
        }
    }
    void FireWeapon()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && weapon != null)
        {
            ShootWeapon();

            SoundWave();
        }

        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire && weapon != null)
        {
            ShootWeapon();

            SoundWave();
        }
    }
    public void ShotgunScatter()
    {
        randomDir = fpsCam.transform.forward;
        randomDir += Random.Range(-scattering, scattering) * transform.right;
    }

    public void SoundWave()
    {
        //hopen dat dit niet teveel shit gaat eisen
        foreach (Transform child in areaColParent.transform)
        {
            if (child.gameObject.activeSelf == true)
            {
                print(child.GetComponent<SphereCollider>());
                child.GetComponent<AreaColScript>().IncreaseSizeStart(weapon.weaponPrefab.GetComponent<GunScript>().weapon.soundAreaIncrease);
            }
        }
    }
    void ReloadWeapon()
    {
        if (weapon.weaponPrefab.GetComponent<GunScript>().weapon.gunType == "Pistol")
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
        }

        if (weapon.weaponPrefab.GetComponent<GunScript>().weapon.gunType == "Shotgun")
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

        if (weapon.weaponPrefab.GetComponent<GunScript>().weapon.gunType == "Sniper")
        {
            if (ammoScript.sniperAmmo <= 0)
            {
                print("NoAmmo");
                return;
            }
            else
            {
                ammoScript.sniperAmmo += currentSlot.ammoInMag;
                if (ammoScript.sniperAmmo >= weapon.weaponPrefab.GetComponent<GunScript>().weapon.magCount)
                {
                    addAmmo = weapon.weaponPrefab.GetComponent<GunScript>().weapon.magCount;
                }
                else
                {
                    addAmmo = ammoScript.sniperAmmo;
                }
                ammoScript.sniperAmmo -= addAmmo;
                ammoScript.UpdateSniperAmmoLeft();
            }
        }

        if (weapon.weaponPrefab.GetComponent<GunScript>().weapon.gunType == "Freezegun")
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
    }
    public IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        //pistolAnimation.SetTrigger("Reload");

        yield return new WaitForSeconds(weapon.weaponPrefab.GetComponent<GunScript>().weapon.reloadSpeed);

        print("Reloaded");
        currentSlot.ammoInMag = addAmmo;
        ammoScript.UpdateAmmo(currentSlot.ammoInMag);
        isReloading = false;
    }
    public IEnumerator HitMarker()
    {
        hitMarkerObj.SetActive(true);
        yield return new WaitForSeconds(displayTimeHitMarker);
        hitMarkerObj.SetActive(false);
    }
    void ShootWeapon()
    {
        if (currentSlot.ammoInMag <= 0 || weaponWheel.activeSelf == true)
        {
            return;
        }

        nextTimeToFire = Time.time + 1f / weapon.weaponPrefab.GetComponent<GunScript>().weapon.fireRate;

        if (weapon.weaponPrefab.GetComponent<GunScript>().weapon.gunType == "Pistol")
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

                GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 2f);
            }

            //pistolAnimation.SetBool("Shoot", false);
        }

        if (weapon.weaponPrefab.GetComponent<GunScript>().weapon.gunType == "Shotgun")
        {
            ShotgunScatter();

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

        if (weapon.weaponPrefab.GetComponent<GunScript>().weapon.gunType == "Sniper")
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

        if (weapon.weaponPrefab.GetComponent<GunScript>().weapon.gunType == "Freezegun")
        {
            //freezegunAnimation.SetBool("Shoot", true);

            currentSlot.ammoInMag--;
            ammoScript.UpdateAmmo(currentSlot.ammoInMag);

            //weapon.muzzleFlash.Play();
            foreach (GameObject zombie in freezeBox.Zombies)
            {
                hitMarkerObj = whiteHitMarkerObj;
                StopCoroutine(coroutine);
                coroutine = HitMarker();
                StartCoroutine(coroutine);

                zombieAnimator.SetFloat("speed", Mathf.Lerp(1, 0, timeUntilFrozen * Time.deltaTime));
                zombieAnimator.SetFloat("Vector1_76374516", Mathf.Lerp(-2, 2, timeUntilFrozen * Time.deltaTime));
            }

            //GameObject impactGO = Instantiate(weapon.impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            //Destroy(impactGO, 2f);

            //freezegunAnimation.SetBool("Shoot", false);
        }
    }
}