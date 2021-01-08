using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    public BossZombie bossScript;
    public void Break()
    {
        GetComponent<Collider>().enabled = !enabled;
        GetComponentInParent<Animator>().SetTrigger("Break");
        StartCoroutine(bossScript.Trigger());
    }
}
