﻿using UnityEngine;

[CreateAssetMenu(fileName = "NewGun", menuName = "Gun")]
public class Gun : Weapon
{
    public bool shootsProjectile;
    public GameObject bulletPrefab;
    public string gunType;
    public string shotType; //FullAuto //One-Round //Burst
    public int slugBulletCount;

    public bool infinityAmmo;
    public string ammoType;
    public int magCount;
    public float bulletSpeed;
    public float reloadSpeed;
    public float fireRate;

    public float soundAreaIncrease;

    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
}
