using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float speed;
    public int damage;
    GameObject playerObj;
    bool isAttacking;
    NavMeshAgent agent;

    public float hitCooldownTime;
    public bool isColliding;
    bool hitCooldown;
    private void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }
    private void Update()
    {
        if(isAttacking == true)
        {
            agent.SetDestination(playerObj.transform.position);
        }
    }
    public void Trigger(GameObject player)
    {
        playerObj = player;
        isAttacking = true;
    }
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isColliding = true;
            StartCoroutine(Hit(collision.gameObject));
        }
    }
    public void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isColliding = false;
        }
    }
    IEnumerator Hit(GameObject player)
    {
        if (isColliding == true && hitCooldown == false)
        {
            hitCooldown = true;
            agent.speed = 0;
            agent.velocity = Vector3.zero;
            //attackAnim
            player.GetComponent<HealthManager>().DoDamage(damage);
            yield return new WaitForSeconds(hitCooldownTime);
            agent.speed = speed;
            hitCooldown = false;
            StartCoroutine(Hit(player));
        }
    }
}
