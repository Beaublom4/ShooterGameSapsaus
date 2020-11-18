using UnityEngine;

public class Weapon : ScriptableObject
{
    public GameObject weaponPrefab;

    public string weaponName;
    [TextArea]
    public string weaponDescription;

    public float damage;
    public float damageDropOverDist;
    public float damageOverTime;
    public float damageOverTimeTime = 2;

    public bool canStun;
    public float stunTime;
    public bool canSlow;
    public float slowTimesNumber;
    public float slowTime;
}
