using UnityEngine;
using System.Collections;

public class AreaColScript : MonoBehaviour
{
    public bool isAlreadyTriggered;
    public float normalColRadius;
    IEnumerator coroutine;
    private void Start()
    {
        normalColRadius = GetComponent<SphereCollider>().radius;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isAlreadyTriggered == false)
        {
            if (other.gameObject.tag == "Player")
            {
                TriggerArea(other.gameObject);
            }
        }
    }
    public void TriggerArea(GameObject player)
    {
        isAlreadyTriggered = true;
        foreach (Transform child in transform)
        {
            child.GetComponent<Enemy>().Trigger(player);
        }
    }
    public void IncreaseSizeStart(float size)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = IncreaseSize(size);
        StartCoroutine(coroutine);
    }
    IEnumerator IncreaseSize(float size)
    {
        GetComponent<SphereCollider>().radius = normalColRadius + size;
        yield return new WaitForSeconds(3);
        GetComponent<SphereCollider>().radius = normalColRadius;
    }
}
