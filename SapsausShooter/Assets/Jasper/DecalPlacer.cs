using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalPlacer : MonoBehaviour
{
    public GameObject decalPrefab;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if bullet Hits Collider

        //SpawnDecal(hitInfo);
    }

    private void SpawnDecal(RaycastHit hitInfo)
    {
        var decal = Instantiate(decalPrefab);
        decal.transform.position = hitInfo.point;
        decal.transform.forward = hitInfo.normal * -1;

    }
}
