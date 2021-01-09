using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWalkSound : MonoBehaviour
{
    public BossZombie bossZombieScript;
    public void WalkSound()
    {
        bossZombieScript.WalkSound();
    }
}
