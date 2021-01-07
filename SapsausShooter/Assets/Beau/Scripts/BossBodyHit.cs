using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBodyHit : MonoBehaviour
{
    public BossZombie bossScript;
    public int bodyType;
    public void HitPart(Weapon weapon, Vector3 hitLoc)
    {
        bossScript.GetDamage(weapon, bodyType, hitLoc);
    }
}
