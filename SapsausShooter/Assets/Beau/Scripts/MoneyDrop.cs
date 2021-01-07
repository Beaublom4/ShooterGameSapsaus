﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyDrop : MonoBehaviour
{
    public int moneyAmount;
    public Material low, medium, lot;
        public LayerMask hitable;
    private void Start()
    {
        if(moneyAmount <= 20)
        {
            GetComponent<MeshRenderer>().material = low;
        }
        else if (moneyAmount > 20 && moneyAmount <= 50)
        {
            GetComponent<MeshRenderer>().material = medium;
        }
        else if (moneyAmount > 50)
        {
            GetComponent<MeshRenderer>().material = lot;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUpCol")
        {
            other.GetComponentInParent<UsePlayer>().PlayAudioSource(other.GetComponentInParent<UsePlayer>().sounds.itemPickUp);
            other.GetComponentInParent<MoneyManager>().GetMoney(moneyAmount);
            Destroy(gameObject);
        }
    }
}
