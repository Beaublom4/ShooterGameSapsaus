using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogZombie : Enemy
{
    public bool walkToPlayer, keepMainPos = true, lookAtPlayer;
    public Transform normalPos;
    public override void Update()
    {
        base.Update();
        if(lookAtPlayer == true)
        {
            transform.LookAt(new Vector3(playerObj.transform.position.x, transform.position.y, playerObj.transform.position.z));
        }
        if(keepMainPos == true)
        {
            agent.destination = normalPos.position;
            if(lookAtPlayer == false && Vector3.Distance(transform.position, normalPos.transform.position) < .5f)
            {
                lookAtPlayer = true;
                normalPos.GetComponentInParent<BossZombie>().RandomAttack();
            }
        }
        else if (walkToPlayer == true)
        {
            agent.destination = playerObj.transform.position;
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            StartCoroutine(Dead(1));
        }
    }
    public override void Trigger(GameObject player)
    {
        if (playerObj == null)
        {
            playerObj = player;
        }
        keepMainPos = false;
        lookAtPlayer = true;
        walkToPlayer = true;
    }
    public override IEnumerator Dead(int hitPoint)
    {
        walkToPlayer = false;
        lookAtPlayer = false;
        keepMainPos = true;
        yield return new WaitForSeconds(1);
    }
}
