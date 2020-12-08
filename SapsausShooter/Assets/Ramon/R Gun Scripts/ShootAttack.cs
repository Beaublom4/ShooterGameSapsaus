using UnityEngine;
using System.Collections;
using TMPro;

public class ShootAttack : MonoBehaviour
{
    public GameObject impactEffect, impactEffect1, impactEffect2, impactEffect3;
    public Weapon weapon;
    public Camera fpsCam;
    //public Animator spAnimator;
    public GameObject areaColParent;
    public GameObject whiteHitMarkerObj, redHitMarkerObj, hitMarkerObj, weaponWheel;

    public LayerMask canHit;

    public Vector3 randomDir;

    public float freezeSpeed = 1f;
    public float nextTimeToFire = 0f;
    public float scattering = 1;

    public int shotPellets = 8;

    public bool canShoot;
    public bool isReloading = false;

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
    }
    void OnEnable()
    {
        isReloading = false;
    }
    public virtual void Update()
    {
        if (weapon.weaponPrefab.GetComponent<GunScript>().weapon.gunType == "Pistol")
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
    }
    public virtual void FireWeapon()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire && weapon != null)
        {
            FireRateAndSwitch();

            ShootWeapon();

            SoundWave();
        }
    }
    public void FireRateAndSwitch()
    {
        if (currentSlot.ammoInMag <= 0 || weaponWheel.activeSelf == true)
        {
            return;
        }

        nextTimeToFire = Time.time + 1f / weapon.weaponPrefab.GetComponent<GunScript>().weapon.fireRate;
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
    public virtual void ReloadWeapon()
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
    public virtual void ShootWeapon()
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
}