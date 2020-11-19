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
                other.GetComponent<Enemy>().DoDamage(meleeWeapon);

                other.GetComponent<Enemy>().isAttacking = false;
                other.GetComponent<NavMeshAgent>().enabled = !enabled;
                other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                other.GetComponent<Rigidbody>().AddRelativeForce(Vector3.back * meleeWeapon.knockBack);
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
        enemy.GetComponent<NavMeshAgent>().enabled = enabled;
        enemy.GetComponent<Enemy>().isAttacking = true;
        enemy.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
}
