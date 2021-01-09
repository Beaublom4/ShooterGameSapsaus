using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMelleeHitboxScript : MonoBehaviour
{
    public BossZombie bossScript;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PickUpCol")
        {
            bossScript.HitWithSpin();
        }
    }
}
