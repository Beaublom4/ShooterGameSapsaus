using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyHit : MonoBehaviour
{
    public int bodyType;
    public Enemy enemyScript;
    private void Start()
    {
        enemyScript = GetComponentInParent<Enemy>();
    }
    public void HitPart(Weapon weapon, Vector3 hitLoc)
    {
        if (enemyScript != null)
        {
            enemyScript.DoDamage(weapon, bodyType, hitLoc);
        }
        else if (GetComponentInParent<BigBabyMiniBoss>())
        {
            GetComponentInParent<BigBabyMiniBoss>().DoDamage(weapon, bodyType, hitLoc);
        }
    }
}
