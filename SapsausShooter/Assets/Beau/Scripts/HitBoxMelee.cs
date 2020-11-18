using UnityEngine;

public class HitBoxMelee : MonoBehaviour
{
    public Melee meleeWeapon;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            if (meleeWeapon != null)
            {
                other.GetComponent<Enemy>().DoDamage(meleeWeapon);
            }
            else
            {
                print("No melee weapon assigned");
            }
        }
    }
}
