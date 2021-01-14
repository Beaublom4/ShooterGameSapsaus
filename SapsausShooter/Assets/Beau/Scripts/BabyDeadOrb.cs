using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyDeadOrb : MonoBehaviour
{
    public GameObject location;
    public float orbSpeed;
    bool hasRunned;
    private void Update()
    {
        //transform.position = Vector3.Lerp(transform.position, location.transform.position, orbSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, location.transform.position, orbSpeed * Time.deltaTime);
        if(Vector3.Distance(transform.position, location.transform.position) < .01f)
        {
            if (hasRunned == false)
            {
                hasRunned = true;
                location.GetComponent<EasterEggBox>().AddBaby();
                Destroy(gameObject, 3);
            }
        }
    }
}
