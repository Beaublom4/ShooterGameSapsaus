using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushedZombie : Enemy
{
    public bool canBash, runOnce;
    public float reLocatePlayerTime, timeBetweenbashes;
    float timer;
    public float bashSpeed;
    public Transform walkToLoc;
    public GameObject bashRangeObj;
    public override void Update()
    {
        base.Update();
        if (canBash == true && runOnce == false)
        {
            runOnce = true;
            print("Bash");
            anim.SetTrigger("TargetFound");
            agent.speed = bashSpeed;
            timer = reLocatePlayerTime;
            agent.SetDestination(walkToLoc.position);
        }
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if(timer < 0)
        {
            timer = reLocatePlayerTime;
            transform.LookAt(playerObj.transform.position);
            agent.SetDestination(walkToLoc.position);
        }
    }
    public override void Trigger(GameObject player)
    {
        base.Trigger(player);
        bashRangeObj.SetActive(true);
    }
    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isColliding = true;
            playerObj = collision.gameObject;
            StartCoroutine(Hit());
            StartCoroutine(HitBash());
        }
        else
        {
            if (collision.gameObject.tag != "Floor")
            {
                print("hit bash");
                StartCoroutine(HitBash());
            }
        }
    }
    IEnumerator HitBash()
    {
        canBash = false;
        runOnce = false;
        bashRangeObj.SetActive(false);
        anim.SetTrigger("TargetHit");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length);
        agent.speed = speed;
        yield return new WaitForSeconds(timeBetweenbashes);
        bashRangeObj.SetActive(true);
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
