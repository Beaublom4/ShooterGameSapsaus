using UnityEngine;
using UnityEngine.UI;

public class Weapon : ScriptableObject
{ 
    public GameObject weaponPrefab;
    public string type;

    public string weaponName;
    [TextArea]
    public string weaponDescription;
    public Sprite uiSprite;

    public float damage;
    public float damageDropOverDist;
    public float damageOverTime;
    public float damageOverTimeTime = 2;
    public float critMultiplier;

    public bool canStun;
    public float stunTime;
    public bool canSlow;
    public float slowTimesNumber;
    public float slowTime;
}
