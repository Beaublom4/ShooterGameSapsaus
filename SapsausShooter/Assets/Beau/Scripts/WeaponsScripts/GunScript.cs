using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public Gun weapon;
    public Transform prefabSpawn;
    public float ammoInMag;
    public GameObject shownBullet;

    private void Start()
    {
        if(ammoInMag > 0)
        {
            if(shownBullet != null)
            shownBullet.SetActive(true);
        }
    }
}
