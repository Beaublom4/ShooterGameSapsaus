using UnityEngine;
using System.Collections;

public class ShootAttack : MonoBehaviour
{
    public Gun weapon;
    public Camera fpsCam;
    public Animator pistolAnimation;

    public LayerMask canHit;

    private float nextTimeToFire = 0f;
    public int currentMagCount;

    public bool canShoot;
    public bool isReloading = false;

    void Start()
    {
        currentMagCount = weapon.magCount;

        canShoot = true;
    }

    void OnEnable()
    {
        isReloading = false;
        pistolAnimation.SetBool("Reloading", false);
    }

    void Update()
    {
        if (isReloading)
        {
            return;
        }

        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire && weapon != null)
        {
            if (currentMagCount <= 0)
            {
                StartCoroutine(Reload());

                return;
            }

            nextTimeToFire = Time.time + 1f / weapon.fireRate;

            Shoot();
        }
    }

    IEnumerator Reload ()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        pistolAnimation.SetBool("Reload", true);

        yield return new WaitForSeconds(weapon.reloadSpeed - .25f);

        pistolAnimation.SetBool("Reload", false);

        yield return new WaitForSeconds(.25f);

        currentMagCount = weapon.magCount;
        isReloading = false;
    }

    void Shoot()
    {
        pistolAnimation.SetBool("Shoot", true);

        currentMagCount--;

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

        pistolAnimation.SetBool("Shoot", false);
    }
}