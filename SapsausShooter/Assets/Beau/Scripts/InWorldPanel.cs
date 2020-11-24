using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InWorldPanel : MonoBehaviour
{
    public Transform facePlayer;

    private void Update()
    {
        transform.LookAt(facePlayer);
    }
}
