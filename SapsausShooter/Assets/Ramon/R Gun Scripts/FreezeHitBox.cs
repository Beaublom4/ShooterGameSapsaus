using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeHitBox : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();
    public float timer;
    public bool ableToDoShit;
    private void Update()
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
                    g.GetComponent<Enemy>().freezeSpeed += .1f;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (ableToDoShit == true)
        {
            if (other.gameObject.tag == "Enemy")
            {
                GameObject g = other.GetComponent<BodyHit>().enemyScript.gameObject;
                if (g.GetComponent<Enemy>().inFreezeRange == false)
                {
                    enemies.Add(g);
                    g.GetComponent<Enemy>().inFreezeRange = true;
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            coroutine = WaitForDelete(other.gameObject);
            StartCoroutine(coroutine);
        }
    }
    IEnumerator coroutine;
    IEnumerator WaitForDelete(GameObject g)
    {
        yield return new WaitForSeconds(.2f);
        g.GetComponent<BodyHit>().enemyScript.freezeSpeed = 0;
        enemies.Remove(g.GetComponent<BodyHit>().enemyScript.gameObject);
        g.GetComponent<BodyHit>().enemyScript.inFreezeRange = false;
    }
}
