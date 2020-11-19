using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBoxMelee : MonoBehaviour
{
    public Melee meleeWeapon;
    public GameObject boss;
    public GameObject player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (meleeWeapon != null)
            {
                player = other.gameObject;
                other.GetComponent<HealthManager>().DoDamage(meleeWeapon.damage);
                GetComponent<Collider>().enabled = !enabled;
            }
        }
    }
}
