using UnityEngine;
using System.Collections;
using TMPro;

public class ShootAttack : MonoBehaviour
{
    public Weapon weapon;
    public PistolShoot pistol;
    public ShotgunShoot shotgun;
    public SniperShoot sniper;
    public FreezegunShoot freezegun;
    public Camera fpsCam;
    //public Animator spAnimator;
    public Mesh freezegunCollider;
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
    void Update()
    {
        if (isReloading)
        {
            return;
        }

        if (Input.GetButtonDown("Fire1"))
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
}