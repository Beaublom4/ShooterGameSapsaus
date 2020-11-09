using System.Security.Permissions;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public GameObject weaponPrefab;
    public bool shootsProjectile;
    public GameObject bulletPrefab;

    public string weaponName;
    public string weaponType;
    [TextArea]
    public string weaponDescription;

    public string ammoType;
    public int magCount;
    public float bulletSpeed;
    public float reloadSpeed;
    public float fireRate;
    public float damage;
    public float damageDropOverDist;
}
