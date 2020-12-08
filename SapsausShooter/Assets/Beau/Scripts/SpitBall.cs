using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitBall : MonoBehaviour
{
    [HideInInspector] public float damage;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<HealthManager>().DoDamage(damage);
        }
        if(collision.gameObject.tag != "Enemy") 
        Destroy(gameObject);
    }
}
