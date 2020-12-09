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

                other.GetComponentInParent<Enemy>().isAttacking = false;
                other.GetComponentInParent<NavMeshAgent>().enabled = !enabled;
                other.GetComponentInParent<Rigidbody>().constraints = RigidbodyConstraints.None;
                other.GetComponentInParent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                other.GetComponentInParent<Rigidbody>().AddRelativeForce(Vector3.back * meleeWeapon.knockBack);
                StartCoroutine(ResetKnockBack(other.gameObject));
            }
            else
            {
                print("No melee weapon assigned");
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
