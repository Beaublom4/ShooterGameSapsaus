using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    public Material dissolveMat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Dead()
    {
        dissolveMat.SetFloat("Vector1_4FF20CCE", 1);
    }
}
