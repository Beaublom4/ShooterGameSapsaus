using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxScroll : MonoBehaviour
{
    public Material skyBoxShaderMaterial;
    public float woosh,wooshKeer;
    // Start is called before the first frame update
    void Start()
    {
        //Invoke("scroll", (Mathf.Lerp(minimum, maximum, t));
    }

    // Update is called once per frame
    void Update()
    {
        woosh += Time.time *wooshKeer;
        skyBoxShaderMaterial.SetFloat("Vector1_B483EFBD", woosh);

        //"Vector1_B483EFBD"
    }
}
