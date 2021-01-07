using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShockWave : MonoBehaviour
{
    public BossZombie bossScript;
    private void OnParticleCollision(GameObject other)
    {
        if(other.gameObject.tag == "PickUpCol")
        other.GetComponentInParent<HealthManager>().DoDamage(bossScript.shockWaveParticleDmg);
    }
}
