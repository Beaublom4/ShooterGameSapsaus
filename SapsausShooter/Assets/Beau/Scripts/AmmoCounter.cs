using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoCounter : MonoBehaviour
{
    public float pistolAmmo;
    public TextMeshProUGUI ammoLeftText, magText;
    public void UpdatePistolAmmoLeft()
    {
        ammoLeftText.text = pistolAmmo.ToString();
    }
    public void UpdateAmmo(float ammo)
    {
        magText.text = ammo.ToString();
    }
}
