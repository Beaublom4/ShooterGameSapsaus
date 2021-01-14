using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeHitBox : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();
    public float timer;
    public bool ableToDoShit;
    public WeaponSelector weaponScript;
    public ParticleSystem cantFreezeParticle;
    
    private void LateUpdate()
    {
        if (ableToDoShit == true)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else if (timer <= 0)
            {
                timer = 1;
                foreach (GameObject g in enemies)
                {
                     if (g.GetComponentInParent<Enemy>().isDeath == false)
                    {
                        g.GetComponentInParent<Enemy>().freezeSpeed += .5f;
                        g.GetComponentInParent<Enemy>().freezeScript = GetComponent<FreezeHitBox>();
                        g.GetComponentInParent<Enemy>().freezeWeapon = weaponScript.selectedSlotScript.gunWeapon;
                    }
                }
            }
        }
        else
        {
            if(enemies.Count > 0)
            {
                enemies[0].GetComponentInParent<Enemy>().inFreezeRange = false;
                enemies.Remove(enemies[0].gameObject);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "FreezeCol" && !other.GetComponentInParent<Enemy>())
        {
            if(cantFreezeParticle.isPlaying == false)
            {
                print("Freeze");
                cantFreezeParticle.Play();
            }
        }
        if (other == null)
        {
            return;
        }
        if (other.gameObject.tag == "FreezeCol")
        {
            if (other.GetComponentInParent<Enemy>())
            {
                if (other.GetComponentInParent<Enemy>().isDeath == true)
                {
                    enemies.Remove(other.gameObject);
                }
            }
        }
        if (ableToDoShit == true)
        {
            if (other.gameObject.tag == "FreezeCol")
            {
                if (other.GetComponentInParent<Enemy>())
                    if (other.GetComponentInParent<Enemy>().inFreezeRange == false)
                    {
                        enemies.Add(other.gameObject);
                        other.GetComponentInParent<Enemy>().inFreezeRange = true;
                    }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "FreezeCol")
        {
            if (other.GetComponentInParent<Enemy>())
            {
                enemies.Remove(other.gameObject);
                other.GetComponentInParent<Enemy>().inFreezeRange = false;
            }
        }
    }
    IEnumerator coroutine;
    IEnumerator WaitForDelete(GameObject g)
    {
        yield return new WaitForSeconds(1f);
        g.GetComponent<BodyHit>().enemyScript.freezeSpeed = 0;
        enemies.Remove(g.GetComponent<BodyHit>().enemyScript.gameObject);
        g.GetComponent<BodyHit>().enemyScript.inFreezeRange = false;
    }
}
