using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushedZombie : Enemy
{
    [HideInInspector] public bool canBash, runOnce, test;
    public float reLocatePlayerTime;
    float timer;
    public float bashSpeed;
    public Transform walkToLoc;
    public override void Update()
    {
        base.Update();
        if (canBash == true && runOnce == false)
        {
            runOnce = true;
            print("Bash");
            anim.SetTrigger("TargetFound");
            agent.speed = bashSpeed;
            test = true;
            timer = reLocatePlayerTime;
            agent.SetDestination(walkToLoc.position);
        }
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if(timer < 0)
        {
            print("test");
            timer = reLocatePlayerTime;
            transform.LookAt(playerObj.transform.position);
            agent.SetDestination(walkToLoc.position);
        }
    }
    public void StartBash()
    {
        if (holdingGun == null)
        {
            canBash = true;
            isAttacking = false;
        }
    }
    public void StopBash()
    {
        if (holdingGun == null)
        {
            canBash = false;
            isAttacking = true;
            timer = 0;
        }
    }
}
