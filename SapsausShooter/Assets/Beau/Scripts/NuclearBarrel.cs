using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuclearBarrel : MonoBehaviour
{
    public float health;
    public float hitDamage;
    public GameObject gasSphere;
    public float gasStayTime;
    bool exploded;
    public void GetDamage(Weapon weapon)
    {
        if (health > 0)
        {
            health -= weapon.damage;
        }
        else if (health <= 0 && exploded == false)
        {
            exploded = true;
            StartCoroutine(Explode());
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Barrel" || collision.collider.tag == "BossHitBox" || collision.collider.isTrigger == true)
        {
            return;
        }
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<HealthManager>().DoDamage(hitDamage);
        }
        exploded = true;
        StartCoroutine(Explode());
    }
    IEnumerator Explode()
    {
        //play explosion anim
        yield return new WaitForSeconds(.1f);
        GetComponent<MeshRenderer>().enabled = !enabled;
        //explosion particles
        Instantiate(gasSphere, transform.position, Quaternion.identity, null);
        Destroy(gameObject);
    }
}
