﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocketExplosion : MonoBehaviour
{
    public GameObject explosionEffect;
    public Collider[] hitColliders;
    public LayerMask explosionLayers;
    public float blastRadius = 5;
    public float explosionForce;
    
    public float speedRocket;
    public bool ifWeCouldFly;
    public float liveTime;
    
    public LayerMask hittableMasks;
    public Gun gunScObj;
    public ShootAttack shootScript;

    public List<Enemy> enemieInRange = new List<Enemy>();
    private void Start()
    {
        Invoke("Explode", liveTime);
    }
    void Update()
    {
        if (ifWeCouldFly)
        {
            transform.Translate(0, -speedRocket * Time.deltaTime, 0);
        }
    }
    public void OnCollisionEnter(Collision rocketCol)
    {
        if (rocketCol.collider.isTrigger == true)
            return;
        if ((hittableMasks & (1<<rocketCol.gameObject.layer)) != 0)
        {
            Explode();
            GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation, null);
            explosion.GetComponent<AudioSource>().Play();
            ifWeCouldFly = false;
            //Explode(rocketCol.contacts[0].point);
        }
        if (rocketCol.collider.tag == "BossHitBox")
        {
            if (rocketCol.collider.GetComponent<BossBodyHit>())
            {
                print("Dikke yeet");
                rocketCol.collider.GetComponent<BossBodyHit>().HitPart(gunScObj, transform.position);
            }
        }
        if(rocketCol.collider.tag == "BreakableWall")
        {
            rocketCol.collider.GetComponent<BreakableWall>().Break();
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "FreezeCol")
        {
            print(other.gameObject.name);
            if (other.GetComponentInParent<Enemy>().isDeath == false || other != null)
            {
                print(other.gameObject.name + " Added");
                enemieInRange.Add(other.GetComponentInParent<Enemy>());
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "FreezeCol")
            enemieInRange.Remove(other.GetComponentInParent<Enemy>());
    }
    void Explode()
    {
        foreach (Enemy enemyScript in enemieInRange)
        {
            print("eeett");
            shootScript.hasShoot = true;
            enemyScript.DoDamage(gunScObj, 2, enemyScript.dmgTextLoc.position);
        }
        if(gameObject != null)
        Destroy(gameObject);
    }

    //public void Explode(Vector3 explosionPoint)
    //{
    //    Instantiate(explosionEffect, transform.position, transform.rotation);

    //    hitColliders = Physics.OverlapSphere(explosionPoint, blastRadius, explosionLayers);

    //    foreach (Collider hitcol in hitColliders)
    //    {
    //        Debug.Log(hitcol.gameObject.name);
    //        if (hitcol.GetComponent<Rigidbody>() != null)
    //        {
    //            hitcol.GetComponent<Rigidbody>().isKinematic = false;
    //            hitcol.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, explosionPoint, blastRadius, 0.2f, ForceMode.Impulse);
    //        }
    //    }
    //}
}
