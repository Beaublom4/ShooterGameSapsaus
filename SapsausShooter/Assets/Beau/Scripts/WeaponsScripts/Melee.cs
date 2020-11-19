using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeWeapon", menuName = "MeleeWeapon")]
public class Melee : Weapon
{
    public GameObject hitBox;
    public float knockBack;
}
