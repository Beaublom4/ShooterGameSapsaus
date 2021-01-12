using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpot : MonoBehaviour
{
    public GameObject player;
    public void Change()
    {
        player.transform.position = transform.position;
    }
}
