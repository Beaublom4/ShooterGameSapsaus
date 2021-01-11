using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.tag);
        if (other.gameObject.tag == "Player")
        {
            GetComponentInParent<Enemy>().HitBoxHit();
        }
        else if(other.gameObject.tag == "")
        {
            GetComponentInParent<Enemy>().HitBoxHit();
        }
    }
}
