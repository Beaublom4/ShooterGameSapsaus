using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GetComponentInParent<Enemy>().HitBoxHit();
        }
    }
}
