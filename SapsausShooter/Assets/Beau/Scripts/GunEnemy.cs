using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEnemy : MonoBehaviour
{
    public Enemy enemyScript;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(enemyScript.holdingGun != null)
            if (enemyScript.isAttacking == true)
            {
                enemyScript.PlayerInShootingRange();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            enemyScript.playerInShootingRange = false;
            if(enemyScript.holdingGun != null) 
            {
                enemyScript.PlayerOutOfShootingRange();              
            }
        }
    }
}
