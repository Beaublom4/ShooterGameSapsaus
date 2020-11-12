using UnityEngine;

[CreateAssetMenu(fileName = "NewGun", menuName = "Gun")]
public class Gun : Weapon
{
    public bool shootsProjectile;
    public GameObject bulletPrefab;
    public string gunType;

    public string ammoType;
    public int magCount;
    public float bulletSpeed;
    public float reloadSpeed;
    public float fireRate;
}
