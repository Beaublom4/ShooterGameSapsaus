﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitBall : MonoBehaviour
{
    [HideInInspector] public float damage;
    private void Start()
    {
        Destroy(gameObject, 5);
    }
    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.tag);
        print(other.gameObject.name);
        if (other.isTrigger == true || other.tag == "Enemy" || other.tag == "FreezeCol" || other.tag == "Weapon")
            return;
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<HealthManager>().DoDamage(damage);
        }
        else if (other.gameObject.tag == "PickUpCol")
        {
            other.gameObject.GetComponentInParent<HealthManager>().DoDamage(damage);
        }
        Destroy(gameObject);
    }
}
