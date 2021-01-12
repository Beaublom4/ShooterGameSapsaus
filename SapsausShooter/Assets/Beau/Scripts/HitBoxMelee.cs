using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class HitBoxMelee : MonoBehaviour
{
    public Melee meleeWeapon;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            if (meleeWeapon != null)
            {
                other.GetComponentInParent<Enemy>().DoDamage(meleeWeapon, 2, new Vector3(other.transform.position.x, other.transform.position.y + 1, other.transform.position.z));
            }
        }
        if(other.gameObject.tag == "BossHitBox")
        {
            if (meleeWeapon != null)
            {
                print(meleeWeapon.damage);
                other.GetComponentInParent<BossZombie>().GetDamage(meleeWeapon, 2, new Vector3(0, -100, 0));
            }
        }
    }
    IEnumerator ResetKnockBack(GameObject enemy)
    {
        yield return new WaitForSeconds(.2f);
        enemy.GetComponentInParent<NavMeshAgent>().enabled = enabled;
        enemy.GetComponentInParent<Enemy>().isAttacking = true;
        enemy.GetComponentInParent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
}
