using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitBall : MonoBehaviour
{
    [HideInInspector] public float damage;
    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<HealthManager>().DoDamage(damage);
        }
        else if(collision.gameObject.tag == "PickUpCol")
        {
            collision.gameObject.GetComponentInParent<HealthManager>().DoDamage(damage);
        }
        if(collision.gameObject.tag != "Enemy") 
        Destroy(gameObject);
    }
}
