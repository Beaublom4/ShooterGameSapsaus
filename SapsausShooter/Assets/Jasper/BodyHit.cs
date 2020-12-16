﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyHit : MonoBehaviour
{
    public int bodyType;
    public Enemy enemyScript;
    private void Start()
    {
        enemyScript = GetComponentInParent<Enemy>();
        if(enemyScript == null)
        {
            print("Probleem poar neem");
        }
    }
    public void HitPart(Weapon weapon, Vector3 hitLoc)
    {
        print(hitLoc);
        enemyScript.DoDamage(weapon, bodyType, hitLoc);
    }
}
