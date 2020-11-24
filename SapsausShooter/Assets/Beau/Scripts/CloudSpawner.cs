using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public Transform point1, point2;
    public float minTimeBetween, maxTimeBetween;
    public GameObject cloudPrefab;
    public float moveSpeed;
    public float maxDistance;

    private void Start()
    {
        StartCoroutine(SpawnCloud());
    }
    private void Update()
    {
        foreach(Transform child in transform)
        {
            child.Translate(0, 0, moveSpeed * Time.deltaTime);
            if(Vector3.Distance(child.position, transform.position) > maxDistance)
            {
                Destroy(child.gameObject);
            }
        }
    }
    IEnumerator SpawnCloud()
    {
        print(point1.position.x);
        float randomX = Random.Range(point1.position.z, point2.position.z);
        print(randomX);
        Instantiate(cloudPrefab, new Vector3(transform.position.x, transform.position.y, randomX), transform.rotation, transform);

        float randomTime = Random.Range(minTimeBetween, maxTimeBetween);
        print("wait for " + randomTime);
        yield return new WaitForSeconds(randomTime);
        StartCoroutine(SpawnCloud());
    }
}
