using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nail : MonoBehaviour
{
    public Vector3 moveDirection;
    public float lifeTime;

    bool move = true;
    private void Update()
    {
        if(move == true)
        transform.Translate(moveDirection * Time.deltaTime);
        Invoke("Destroy", lifeTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject);
        if(collision.gameObject.tag == "Enemy")
        {

        }
        transform.SetParent(collision.transform);
        move = false;
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
