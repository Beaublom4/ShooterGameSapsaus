using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyHit : MonoBehaviour
{
    public int bodyType;
    Enemy enemyScript;
    private void Start()
    {
        enemyScript = GetComponentInParent<Enemy>();
        if(enemyScript == null)
        {
            print("Probleem poar neem");
        }
    }
    public void HitPart(Weapon weapon)
    {
        print("hit");
        enemyScript.DoDamage(weapon, bodyType);
    }
}
