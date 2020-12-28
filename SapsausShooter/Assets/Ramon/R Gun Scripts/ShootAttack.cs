﻿using UnityEngine;
using System.Collections;
using TMPro;

public class ShootAttack : MonoBehaviour
{
    [System.Serializable]
    public class Sounds
    {
        [HideInInspector] public float pistolSoundVolume, shotgunSoundVolume;
        public AudioSource pistolShoot, pistolEmpty, pistolReload;
        public AudioSource shotgunShoot, shotgunEmpty, shotgunReload;

        [HideInInspector] public float hitmarkerSoundVolume;
        public AudioSource hitmarker, headshot;
    }

    public GameObject impactEffect, impactEffect1, impactEffect2, impactEffect3;
    public Weapon weapon;
    public Camera fpsCam;
    public Animator anim;
    public GameObject areaColParent;
    public GameObject whiteHitMarkerObj, redHitMarkerObj, hitMarkerObj, weaponWheel, optionsPanel;
    public GameObject rocketPrefab;
    public Transform weaponHand;
    public Transform myTransform;

    public FreezeHitBox freezeBox;

    public MaterialPropertyBlock block;

    public LayerMask canHit;

    public Vector3 randomDir;

    public float freezeSpeed = 1f;
    public float nextTimeToFire = 0f;
    public float scattering = 1;
    public float timeUntilFrozen = 2f;
    public float timeUntilDefrozen = 5;
    public float dissolvingNumber;
    public float dissolveSpeed;
    public float propulsionForce;

    public int shotPellets = 8;

    public bool canShoot;
    public bool isReloading = false;
    public bool doingFreeze;
    public GameObject freezeColObj;

    public float addAmmo;
    public Slot currentSlot;
    public AmmoCounter ammoScript;
    public MouseLook camScript;

    public float displayTimeHitMarker;
    public IEnumerator coroutine;
    public IEnumerator colCoroutine;

    public Sounds sounds;

    public GameObject recoilObj;
    public float recoilResetTime;
    IEnumerator recoilCooldown;

    public Transform scatteringObj;
    void Start()
    {
        //currentMagCount = weapon.magCount;
        coroutine = HitMarker();
        canShoot = true;

        sounds.pistolSoundVolume = sounds.pistolReload.volume;
        sounds.shotgunSoundVolume = sounds.shotgunReload.volume;
        sounds.hitmarkerSoundVolume = sounds.hitmarker.volume;

        //freezeSpeed = spAnimator.GetFloat("speed");

        SetInstantiateReferences();
    }
    void OnEnable()
    {
        isReloading = false;
    }
    void Update()
    {
        Debug.DrawRay(recoilObj.transform.position, recoilObj.transform.forward * 5, Color.blue);
        if (isReloading || weaponWheel.activeSelf == true || optionsPanel.activeSelf == true)
        {
            return;
        }

        FireWeapon();

        if (Input.GetButtonDown("Reload") && weapon != null && currentSlot.ammoInMag < weapon.weaponPrefab.GetComponent<GunScript>().weapon.magCount)
        {
            ReloadWeapon();

            StartCoroutine(Reload());
        }
        if (doingFreeze == true)
        {
            currentSlot.ammoInMag -= Time.deltaTime;
            ammoScript.UpdateAmmo(currentSlot.ammoInMag);

            freezeColObj.SetActive(true);
        }
    }
    void FireWeapon()
    {
        if (weaponWheel.activeSelf == true)
            return;
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire && weapon != null)
        {
            ShootWeapon();
            SoundWave();
        }
        if (currentSlot != null)
        {
            if (currentSlot.gunWeapon.gunType == "FreezeGun" && Input.GetButton("Fire1"))
            {
                if (currentSlot.ammoInMag > 0)
                {
                    freezeColObj.GetComponent<FreezeHitBox>().ableToDoShit = true;
                    doingFreeze = true;
                }
                else
                {
                    doingFreeze = false;
                    freezeColObj.GetComponent<FreezeHitBox>().ableToDoShit = false;
                }
            }
            else if (doingFreeze == true)
            {
                freezeColObj.GetComponent<FreezeHitBox>().ableToDoShit = false;
                doingFreeze = false;
            }
        }
        //if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && weapon != null)
        //{
        //    ShootWeapon();

        //    SoundWave();
        //}
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
                sounds.pistolReload.volume = Random.Range(sounds.pistolSoundVolume - .1f, sounds.pistolSoundVolume + .1f);
                sounds.pistolReload.pitch = Random.Range(1 - .1f, 1 + .1f);
                sounds.pistolReload.Play();

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
                sounds.shotgunReload.volume = Random.Range(sounds.shotgunSoundVolume - .1f, sounds.shotgunSoundVolume + .1f);
                sounds.shotgunReload.pitch = Random.Range(1 - .1f, 1 + .1f);
                sounds.shotgunReload.Play();

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
        if(currentSlot.gunWeapon.gunType == "Launcher")
        {
            if (ammoScript.launcherAmmo <= 0)
            {
                print("NoAmmo");
                return;
            }
            else
            {
                ammoScript.launcherAmmo += currentSlot.ammoInMag;
                if (ammoScript.launcherAmmo >= weapon.weaponPrefab.GetComponent<GunScript>().weapon.magCount)
                {
                    addAmmo = weapon.weaponPrefab.GetComponent<GunScript>().weapon.magCount;
                }
                else
                {
                    addAmmo = ammoScript.launcherAmmo;
                }
                ammoScript.launcherAmmo -= addAmmo;
                ammoScript.UpdateLauncherAmmoLeft();
            }
        }
        if (currentSlot.gunWeapon.gunType == "FreezeGun")
        {
            if (ammoScript.specialAmmo <= 0)
            {
                print("NoAmmo");
                return;
            }
            else
            {
                ammoScript.specialAmmo += currentSlot.ammoInMag;
                if (ammoScript.specialAmmo >= weapon.weaponPrefab.GetComponent<GunScript>().weapon.magCount)
                {
                    addAmmo = weapon.weaponPrefab.GetComponent<GunScript>().weapon.magCount;
                }
                else
                {
                    addAmmo = ammoScript.specialAmmo;
                }
                ammoScript.specialAmmo -= addAmmo;
                ammoScript.UpdateSpecialAmmoLeft();
            }
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
        if (currentSlot.gunWeapon.gunType == "FreezeGun")
        {
            return;
        }
        nextTimeToFire = Time.time + 1f / weapon.weaponPrefab.GetComponent<GunScript>().weapon.fireRate;

        if (currentSlot.ammoInMag <= 0 || weaponWheel.activeSelf == true)
        {
            if (currentSlot.ammoInMag <= 0)
            {
                if (currentSlot.gunWeapon.gunType == "Pistol")
                {
                    sounds.pistolEmpty.volume = Random.Range(sounds.pistolSoundVolume - .1f, sounds.pistolSoundVolume + .1f);
                    sounds.pistolEmpty.pitch = Random.Range(1 - .1f, 1 + .1f);
                    sounds.pistolEmpty.Play();
                }
                else if (currentSlot.gunWeapon.gunType == "Shotgun")
                {
                    sounds.shotgunEmpty.volume = Random.Range(sounds.shotgunSoundVolume - .1f, sounds.shotgunSoundVolume + .1f);
                    sounds.shotgunEmpty.pitch = Random.Range(1 - .1f, 1 + .1f);
                    sounds.shotgunEmpty.Play();
                }
            }
            return;
        }

        anim.SetTrigger("Shoot");

        if (weapon.weaponPrefab.GetComponent<GunScript>().weapon.gunType == "Pistol")
        {
            //pistolAnimation.SetBool("Shoot", true);

            sounds.pistolShoot.volume = Random.Range(sounds.pistolSoundVolume - .2f, sounds.pistolSoundVolume + .1f);
            sounds.pistolShoot.pitch = Random.Range(1 - .1f, 1 + .1f);
            sounds.pistolShoot.Play();
            currentSlot.ammoInMag--;
            ammoScript.UpdateAmmo(currentSlot.ammoInMag);
            weaponHand.GetComponentInChildren<ParticleSystem>().Play();

            RaycastHit hit;
            if (Physics.Raycast(recoilObj.transform.position, recoilObj.transform.forward, out hit, 1000, canHit, QueryTriggerInteraction.Ignore))
            {
                recoilObj.transform.Rotate(-weapon.recoil, 0, 0);
                if (recoilCooldown != null)
                {
                    StopCoroutine(recoilCooldown);
                }
                recoilCooldown = RecoilReset();
                StartCoroutine(recoilCooldown);
                if (hit.collider.tag == "Enemy")
                {
                    if (hit.collider.GetComponent<BodyHit>())
                    {
                        hit.collider.GetComponent<BodyHit>().HitPart(weapon, hit.point);
                        HitMarker(hit.collider.gameObject);
                    }
                }

                GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 2f);
            }

            //pistolAnimation.SetBool("Shoot", false);
        }

        if (weapon.weaponPrefab.GetComponent<GunScript>().weapon.gunType == "Shotgun")
        {
            sounds.shotgunShoot.volume = Random.Range(sounds.shotgunSoundVolume - .2f, sounds.shotgunSoundVolume + .1f);
            sounds.shotgunShoot.pitch = Random.Range(1 - .1f, 1 + .1f);
            sounds.shotgunShoot.Play();
            weaponHand.GetComponentInChildren<ParticleSystem>().Play();

            currentSlot.ammoInMag--;
            ammoScript.UpdateAmmo(currentSlot.ammoInMag);

            for (int s = 0; s < currentSlot.gunWeapon.slugBulletCount; s++)
            {
                scatteringObj.transform.localRotation = Quaternion.Euler(0, 0, 0);
                scatteringObj.transform.localRotation = Quaternion.Euler(Random.Range(-scattering, scattering), Random.Range(-scattering, scattering), 0);
                RaycastHit hit;
                if (Physics.Raycast(recoilObj.transform.position, scatteringObj.transform.forward, out hit, 1000, canHit, QueryTriggerInteraction.Ignore))
                {
                    if (hit.collider.tag == "Enemy")
                    {
                        if (hit.collider.GetComponent<BodyHit>())
                        {
                            hit.collider.GetComponent<BodyHit>().HitPart(weapon, hit.point);

                            HitMarker(hit.collider.gameObject);
                        }
                    }
                    else
                    {
                        //Instantiate(testObj, hit.point, Quaternion.LookRotation(hit.normal), null);
                    }
                }

            }
            //for (int i = 0; i < Mathf.Max(1, shotPellets); i++)
            //{

            //    RaycastHit hit;
            //    if (Physics.Raycast(recoilObj.transform.position, recoilObj.transform.forward, out hit, 1000, canHit, QueryTriggerInteraction.Ignore))
            //    {
            //        if (hit.collider.tag == "Enemy")
            //        {
            //            if (hit.collider.GetComponent<BodyHit>())
            //            {
            //                hit.collider.GetComponent<BodyHit>().HitPart(weapon, hit.point);

            //                HitMarker(hit.collider.gameObject);
            //            }
            //        }
            //        //GameObject impactGO = Instantiate(weapon.impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            //        //Destroy(impactGO, 2f);
            //    }
            //}
            //shotgunAnimation.SetBool("Shoot", false);
        }

        if (weapon.weaponPrefab.GetComponent<GunScript>().weapon.gunType == "Sniper")
        {
            //sniperAnimation.SetBool("Shoot", true);

            currentSlot.ammoInMag--;
            ammoScript.UpdateAmmo(currentSlot.ammoInMag);

            //weapon.muzzleFlash.Play();
            RaycastHit hit;
            if (Physics.Raycast(recoilObj.transform.position, recoilObj.transform.forward, out hit, 1000, canHit, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.tag == "Enemy")
                {
                    if (hit.collider.GetComponent<BodyHit>())
                    {
                        hit.collider.GetComponent<BodyHit>().HitPart(weapon, hit.point);

                        HitMarker(hit.collider.gameObject);
                    }
                }

                //GameObject impactGO = Instantiate(weapon.impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                //Destroy(impactGO, 2f);
            }

            //sniperAnimation.SetBool("Shoot", false);
        }

        if (weapon.weaponPrefab.GetComponent<GunScript>().weapon.gunType == "Launcher")
        {
            currentSlot.ammoInMag--;
            ammoScript.UpdateAmmo(currentSlot.ammoInMag);

            GameObject rocket = (GameObject)Instantiate(rocketPrefab, weaponHand.GetComponentInChildren<GunScript>().prefabSpawn.position, weaponHand.GetComponentInChildren<GunScript>().prefabSpawn.rotation, null);
            rocket.GetComponent<rocketExplosion>().ifWeCouldFly = true;

            //GameObject impactGO = Instantiate(weapon.impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            //Destroy(impactGO, 2f);

            //rpgAnimation.SetBool("Shoot", false);
        }
    }
    public void HitMarker(GameObject obj)
    {
        if (obj.GetComponent<BodyHit>().bodyType == 1)
        {
            hitMarkerObj = redHitMarkerObj;
            sounds.headshot.volume = Random.Range(sounds.hitmarkerSoundVolume - .1f, sounds.hitmarkerSoundVolume + .1f);
            sounds.headshot.pitch = Random.Range(1 - .1f, 1 + .1f);
            sounds.headshot.Play();
        }
        else
        {
            hitMarkerObj = whiteHitMarkerObj;
            sounds.hitmarker.volume = Random.Range(sounds.hitmarkerSoundVolume - .1f, sounds.hitmarkerSoundVolume + .1f);
            sounds.hitmarker.pitch = Random.Range(1 - .1f, 1 + .1f);
            sounds.hitmarker.Play();
        }
        StopCoroutine(coroutine);
        coroutine = HitMarker();
        StartCoroutine(coroutine);
    }
    void SetInstantiateReferences()
    {
        myTransform = transform;
    }
    IEnumerator RecoilReset()
    {
        yield return new WaitForSeconds(recoilResetTime);
        recoilObj.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
}