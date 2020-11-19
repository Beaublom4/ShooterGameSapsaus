using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeWeapon", menuName = "MeleeWeapon")]
public class Melee : Weapon
{
    public GameObject hitBox, hitBoxForEnemy;
    public float knockBack;
}
