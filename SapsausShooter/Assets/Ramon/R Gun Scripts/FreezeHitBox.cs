using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeHitBox : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();
    public float timer;
    public bool ableToDoShit;
    public WeaponSelector weaponScript;
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
        print("1");
        if(other == null)
        {
            return;
        }
        if (other.gameObject.tag == "FreezeCol")
        {
            print("2");
            if (other.GetComponentInParent<Enemy>().isDeath == true)
            {
                print("3");
                enemies.Remove(other.gameObject);
            }
        }
        if (ableToDoShit == true)
        {
            if (other.gameObject.tag == "FreezeCol")
            {
                if (other.GetComponentInParent<Enemy>().inFreezeRange == false)
                {
                    enemies.Add(other.gameObject);
                    other.GetComponentInParent<Enemy>().inFreezeRange = true;
                }

                //GameObject g = other.GetComponent<BodyHit>().enemyScript.gameObject;
                //if (g.GetComponent<Enemy>().inFreezeRange == false)
                //{
                //    enemies.Add(g);
                //    g.GetComponent<Enemy>().inFreezeRange = true;
                //}
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "FreezeCol")
        {
            enemies.Remove(other.gameObject);
            other.GetComponentInParent<Enemy>().inFreezeRange = false;

            //if (coroutine != null)
            //{
            //    StopCoroutine(coroutine);
            //}
            //coroutine = WaitForDelete(other.gameObject);
            //StartCoroutine(coroutine);
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
