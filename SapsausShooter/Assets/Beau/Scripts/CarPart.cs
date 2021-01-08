using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPart : MonoBehaviour
{
    public AudioSource carAlarm;
    public GameObject[] wave1;
    public Transform wave1Spawn;
    public GameObject[] wave2;
    public Transform wave2Spawn;
    GameObject playerObj;
    bool hasTriggered;
    private void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter(Collider trigger)
    {
        if(trigger.gameObject.tag == "Player")
        {
            if (hasTriggered == false)
            {
                hasTriggered = true;
                StartCoroutine(CarAlarm());
            }
        }
    }
    IEnumerator CarAlarm()
    {
        carAlarm.Play();
        foreach(GameObject enemy in wave1)
        {
            GameObject g = Instantiate(enemy, wave1Spawn.position, wave1Spawn.rotation, wave1Spawn);
            g.GetComponent<Enemy>().Trigger(playerObj);
        }
        yield return new WaitForSeconds(10);
        foreach (GameObject enemy in wave2)
        {
            GameObject g = Instantiate(enemy, wave2Spawn.position, wave2Spawn.rotation, wave2Spawn);
            g.GetComponent<Enemy>().Trigger(playerObj);
        }
        yield return new WaitForSeconds(10);
        carAlarm.Stop();
    }
}
