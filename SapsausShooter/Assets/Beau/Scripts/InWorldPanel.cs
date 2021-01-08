using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InWorldPanel : MonoBehaviour
{
    public Transform facePlayer;

    private void Start()
    {
        facePlayer = GameObject.FindGameObjectWithTag("FacePlayer").transform;
    }
    private void Update()
    {
        transform.LookAt(facePlayer);
    }
}
