﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nail : MonoBehaviour
{
    public Vector3 moveDirection;
    public float lifeTime;
    public Weapon weapon;
    public GameObject hitVfx;

    bool move = true;
    private void Update()
    {
        if(move == true)
        transform.Translate(moveDirection * Time.deltaTime);
        Invoke("Destroy", lifeTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject);
        if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<BodyHit>().HitPart(weapon, transform.position);
            GameObject g = Instantiate(hitVfx, transform.position, transform.rotation, null);
            Destroy(g, 3);
        }
        transform.SetParent(collision.transform);
        move = false;
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
