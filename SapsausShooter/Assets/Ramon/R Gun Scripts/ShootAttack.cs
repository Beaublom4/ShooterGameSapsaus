using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.VFX;

public class ShootAttack : MonoBehaviour
{
    [System.Serializable]
    public class Sounds
    {
        [HideInInspector] public float pistolSoundVolume, shotgunSoundVolume;
        public AudioSource pistolShoot, pistolEmpty, pistolReload;
        public AudioSource shotgunShoot, shotgunEmpty, shotgunReload;
        public AudioSource launcherShoot;
        public AudioSource freezeLoopShoot, freezeDry;

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
    public GameObject hitVfx;
    public GameObject PistolMag, ShotgunShells, launcherBullet, freezeBatery;
    public GameObject magLoc, rlMagLoc, blikkieLoc;
    public VoiceLineCol thisOneIsUselessNow;

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
        if (weaponWheel.activeSelf == true || currentSlot == null || Time.time < nextTimeToFire)
            return;
        if (Input.GetButtonDown("Fire1") && weapon != null)
        {
            if(currentSlot.gunWeapon.gunType == "FreezeGun")
            {
                if(currentSlot.ammoInMag <= 0)
                {
                    sounds.freezeDry.Play();
                }
            }
            ShootWeapon();
            SoundWave();
        }
        if (Input.GetButton("Fire1"))
        {
            if (currentSlot.gunWeapon.gunType == "FreezeGun")
            {
                if (currentSlot.ammoInMag > 0)
                {
                    if(sounds.freezeLoopShoot.isPlaying == false)
                    {
                        sounds.freezeLoopShoot.Play();
                    }
                    freezeColObj.GetComponent<FreezeHitBox>().ableToDoShit = true;
                    doingFreeze = true;
                    weaponHand.GetComponentInChildren<VisualEffect>().Play();
                }
                else
                {
                    if (sounds.freezeLoopShoot.isPlaying == true)
                    {
                        sounds.freezeLoopShoot.Stop();
                    }
                    print("stop");
                    doingFreeze = false;
                    freezeColObj.GetComponent<FreezeHitBox>().ableToDoShit = false;
                    weaponHand.GetComponentInChildren<VisualEffect>().Stop();
                }
            }
            else if (doingFreeze == true)
            {
                if (sounds.freezeLoopShoot.isPlaying == true)
                {
                    sounds.freezeLoopShoot.Stop();
                }
                print("stop");
                freezeColObj.GetComponent<FreezeHitBox>().ableToDoShit = false;
                doingFreeze = false;
                weaponHand.GetComponentInChildren<VisualEffect>().Stop();
            }
            if(currentSlot.gunWeapon.gunType == "GatlingNailGun")
            {
                if(currentSlot.ammoInMag <= 0)
                {
                    return;
                }
                nextTimeToFire = Time.time + currentSlot.gunWeapon.fireRate;
                print("Shoot");
                //sounds.pistolShoot.volume = Random.Range(sounds.pistolSoundVolume - .2f, sounds.pistolSoundVolume + .1f);
                //sounds.pistolShoot.pitch = Random.Range(1 - .1f, 1 + .1f);
                //sounds.pistolShoot.Play();
                currentSlot.ammoInMag--;
                ammoScript.UpdateAmmo(currentSlot.ammoInMag);
                //weaponHand.GetComponentInChildren<ParticleSystem>().Play();

                recoilObj.transform.Rotate(-weapon.recoil, 0, 0);
                if (recoilCooldown != null)
                {
                    StopCoroutine(recoilCooldown);
                }
                recoilCooldown = RecoilReset();
                StartCoroutine(recoilCooldown);

                Instantiate(currentSlot.gunWeapon.bulletPrefab, weaponHand.GetComponentInChildren<GunScript>().prefabSpawn.position, recoilObj.transform.rotation, null);
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            if(doingFreeze == true)
            {
                if (sounds.freezeLoopShoot.isPlaying == true)
                {
                    sounds.freezeLoopShoot.Stop();
                }
                print("stop");
                doingFreeze = false;
                freezeColObj.GetComponent<FreezeHitBox>().ableToDoShit = false;
                weaponHand.GetComponentInChildren<VisualEffect>().Stop();
            }
        }
    }
    public void SoundWave()
    {
        //hopen dat dit niet teveel shit gaat eisen
        foreach (Transform child in areaColParent.transform)
        {
            if (child.gameObject.activeSelf == true)
            {
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
                anim.SetTrigger("Reload");
                weaponHand.GetComponentInChildren<Animator>().SetTrigger("ReloadP");
                GameObject mag = Instantiate(PistolMag, magLoc.transform.position, magLoc.transform.rotation, magLoc.transform);
                Destroy(mag, 1);
                ammoScript.pistolAmmo += currentSlot.ammoInMag;
                if (ammoScript.pistolAmmo >= currentSlot.gunWeapon.magCount)
                {
                    print("Stap 1");
                    addAmmo = weapon.weaponPrefab.GetComponent<GunScript>().weapon.magCount;
                }
                else
                {
                    print("Stap 2");
                    addAmmo = ammoScript.pistolAmmo;
                }
                currentSlot.ammoInMag = 0;
                sounds.pistolReload.volume = Random.Range(sounds.pistolSoundVolume - .1f, sounds.pistolSoundVolume + .1f);
                sounds.pistolReload.pitch = Random.Range(1 - .1f, 1 + .1f);
                sounds.pistolReload.Play();

                ammoScript.pistolAmmo -= addAmmo;
                StartCoroutine(Reload());
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
                anim.SetTrigger("Reload");
                GameObject mag = Instantiate(ShotgunShells, magLoc.transform.position, magLoc.transform.rotation, magLoc.transform);
                Destroy(mag, 1);
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
                StartCoroutine(Reload());
                ammoScript.UpdateShotgunAmmoLeft();
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
                anim.SetTrigger("Reload");
                GameObject mag = Instantiate(launcherBullet, rlMagLoc.transform.position, rlMagLoc.transform.rotation, rlMagLoc.transform);
                Destroy(mag, .7f);
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
                StartCoroutine(Reload());
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
                anim.SetTrigger("Reload");
                GameObject mag = Instantiate(freezeBatery, magLoc.transform.position, magLoc.transform.rotation, magLoc.transform);
                Destroy(mag, 1);
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
                StartCoroutine(Reload());
                ammoScript.UpdateSpecialAmmoLeft();
            }
        }
        if (currentSlot.gunWeapon.gunType == "GatlingNailGun")
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
                StartCoroutine(Reload());
                ammoScript.UpdateSpecialAmmoLeft();
            }
        }
    }
    public IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(weapon.weaponPrefab.GetComponent<GunScript>().weapon.reloadSpeed);

        print("Reloaded");
        sounds.pistolReload.volume = Random.Range(sounds.pistolSoundVolume - .1f, sounds.pistolSoundVolume + .1f);
        sounds.pistolReload.pitch = Random.Range(1.5f - .1f, 1.5f + .1f);
        sounds.pistolReload.Play();
        currentSlot.ammoInMag = addAmmo;
        ammoScript.UpdateAmmo(currentSlot.ammoInMag);
        isReloading = false;
        if(currentSlot.gunWeapon.gunType == "Launcher")
        {
            weaponHand.GetComponentInChildren<GunScript>().shownBullet.SetActive(true);
        }
    }
    public IEnumerator HitMarker()
    {
        hitMarkerObj.SetActive(true);
        yield return new WaitForSeconds(displayTimeHitMarker);
        hitMarkerObj.SetActive(false);
    }
    void ShootWeapon()
    {
        if (currentSlot.gunWeapon.gunType == "FreezeGun" || currentSlot.gunWeapon.shotType == "Full-Auto")
        {
            return;
        }
        nextTimeToFire = Time.time + weapon.weaponPrefab.GetComponent<GunScript>().weapon.fireRate;

        if (currentSlot.ammoInMag <= 0 || weaponWheel.activeSelf == true)
        {
            if (currentSlot.ammoInMag <= 0)
            {
                thisOneIsUselessNow.StartSound();
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
            weaponHand.GetComponentInChildren<Animator>().SetTrigger("ShootP");

            sounds.pistolShoot.volume = Random.Range(sounds.pistolSoundVolume - .2f, sounds.pistolSoundVolume + .1f);
            sounds.pistolShoot.pitch = Random.Range(1 - .1f, 1 + .1f);
            sounds.pistolShoot.Play();
            currentSlot.ammoInMag--;
            ammoScript.UpdateAmmo(currentSlot.ammoInMag);
            weaponHand.GetComponentInChildren<ParticleSystem>().Play();

            RaycastHit hit;
            if (Physics.Raycast(recoilObj.transform.position, recoilObj.transform.forward, out hit, 1000, canHit, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.tag == "Enemy")
                {
                    if (hit.collider.GetComponent<BodyHit>())
                    {
                        GameObject g = Instantiate(hitVfx, hit.point, Quaternion.LookRotation(hit.normal), null);
                        Destroy(g, 3);
                        hit.collider.GetComponent<BodyHit>().HitPart(weapon, hit.point);
                        HitMarker(hit.collider.gameObject);
                    }
                }
                else if (hit.collider.tag == "BossHitBox")
                {
                    if (hit.collider.GetComponent<BossBodyHit>())
                    {
                        GameObject g = Instantiate(hitVfx, hit.point, Quaternion.LookRotation(hit.normal), null);
                        Destroy(g, 3);
                        hit.collider.GetComponent<BossBodyHit>().HitPart(weapon, hit.point);
                        HitMarker(hit.collider.gameObject);
                    }
                }
            }
            recoilObj.transform.Rotate(-weapon.recoil, 0, 0);
            if (recoilCooldown != null)
            {
                StopCoroutine(recoilCooldown);
            }
            recoilCooldown = RecoilReset();
            StartCoroutine(recoilCooldown);
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
                            GameObject g = Instantiate(hitVfx, hit.point, Quaternion.LookRotation(hit.normal), null);
                            Destroy(g, 3);
                            hit.collider.GetComponent<BodyHit>().HitPart(weapon, hit.point);

                            HitMarker(hit.collider.gameObject);
                        }
                        else if (hit.collider.tag == "BossHitBox")
                        {
                            if (hit.collider.GetComponent<BossBodyHit>())
                            {
                                GameObject g = Instantiate(hitVfx, hit.point, Quaternion.LookRotation(hit.normal), null);
                                Destroy(g, 3);
                                hit.collider.GetComponent<BossBodyHit>().HitPart(weapon, hit.point);
                                HitMarker(hit.collider.gameObject);
                            }
                        }
                    }
                    else
                    {
                        //Instantiate(testObj, hit.point, Quaternion.LookRotation(hit.normal), null);
                    }
                }
            }
            recoilObj.transform.Rotate(-weapon.recoil, 0, 0);
            if (recoilCooldown != null)
            {
                StopCoroutine(recoilCooldown);
            }
            recoilCooldown = RecoilReset();
            StartCoroutine(recoilCooldown);
        }
        if (weapon.weaponPrefab.GetComponent<GunScript>().weapon.gunType == "Launcher")
        {
            currentSlot.ammoInMag--;
            ammoScript.UpdateAmmo(currentSlot.ammoInMag);

            sounds.launcherShoot.Play();
            GameObject rocket = (GameObject)Instantiate(rocketPrefab, weaponHand.GetComponentInChildren<GunScript>().prefabSpawn.position, weaponHand.GetComponentInChildren<GunScript>().prefabSpawn.rotation, null);
            rocket.GetComponent<rocketExplosion>().ifWeCouldFly = true;

            weaponHand.GetComponentInChildren<GunScript>().shownBullet.SetActive(false);

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