using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBabyMissle : MonoBehaviour
{
    public float damage;
    public GameObject player;
    public float speed;
    public bool move;
    private void Start()
    {
        Invoke("MoveOn", 2);
    }
    private void Update()
    {
        if (move == true)
        {
            transform.LookAt(player.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }
    void MoveOn()
    {
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.GetChild(0).gameObject.SetActive(true);
        move = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger == true || other.tag == "Enemy" || other.tag == "FreezeCol" || other.tag == "Weapon")
            return;
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<HealthManager>().DoDamage(damage);
        }
        else if (other.gameObject.tag == "PickUpCol")
        {
            other.gameObject.GetComponentInParent<HealthManager>().DoDamage(damage);
        }
        Destroy(gameObject);
    }
}
