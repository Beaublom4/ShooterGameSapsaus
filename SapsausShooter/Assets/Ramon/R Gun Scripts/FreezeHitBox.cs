using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeHitBox : MonoBehaviour
{
    public ShootAttack Shoot;
    public List<GameObject> Zombies = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        Zombies.Add(other.gameObject);
    }
}
