using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gas : MonoBehaviour
{
    public float damage;
    public float lifeTime;
    public float addTimeToTimer;
    float timer;
    public bool playerInRange;

    public GameObject playerObj;
    private void Start()
    {
        Invoke("Destroy", lifeTime);
        if (GetComponent<AudioSource>())
        GetComponent<AudioSource>().Play();
    }
    private void Update()
    {
        if(playerInRange == true)
        {
            if(timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else if(timer <= 0)
            {
                playerObj.GetComponent<HealthManager>().DoDamage(damage);
                timer = addTimeToTimer;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerObj = other.gameObject;
            timer = addTimeToTimer;
            playerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerObj = null;
            playerInRange = false;
        }
    }
    void Destroy()
    {
        Destroy(gameObject);
    }
}
