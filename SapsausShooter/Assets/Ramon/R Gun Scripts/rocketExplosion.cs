using System.Collections;
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

    void Update()
    {
        if (ifWeCouldFly)
        {
            transform.Translate(0, -speedRocket * Time.deltaTime, 0);
            print("vliegjonghu");
        }
    }

    public void OnCollisionEnter(Collision rocketCol)
    {
        Debug.Log(rocketCol.contacts[0].point.ToString());
        Explode(rocketCol.contacts[0].point);
    }

    public void Explode(Vector3 explosionPoint)
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);

        hitColliders = Physics.OverlapSphere(explosionPoint, blastRadius, explosionLayers);

        foreach (Collider hitcol in hitColliders)
        {
            Debug.Log(hitcol.gameObject.name);
            if (hitcol.GetComponent<Rigidbody>() != null)
            {
                hitcol.GetComponent<Rigidbody>().isKinematic = false;
                hitcol.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, explosionPoint, blastRadius, 0.2f, ForceMode.Impulse);
            }
        }
    }
}
