using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocketExplosion : MonoBehaviour
{
    public GameObject explosionEffect;
    public float rocketCountdown = 6f;
    public float blastRadius = 5;
    public float force = 700;
    public float speedRocket;
    public bool hasExploded = false, ifWeCouldFly;
    void OnEnable()
    {

    }

    void Update()
    {
        if (rocketCountdown <= 0f && hasExploded)
        {
            print("boom");
            Explode();
            hasExploded = true;
        }
        if (ifWeCouldFly)
        {
            transform.Translate(0, -speedRocket * Time.deltaTime, 0);
            print("vliegjonghu");
        }
    }

    public void Explode()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);

        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, blastRadius);
            }
        }

        Destroy(gameObject);
    }
}
