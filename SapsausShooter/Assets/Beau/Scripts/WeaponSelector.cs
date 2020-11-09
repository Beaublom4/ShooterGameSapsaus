using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelector : MonoBehaviour
{
    public GameObject weaponWheelObj;
    private void Update()
    {
        if (Input.GetButtonDown("WeaponWheel"))
        {
            if (weaponWheelObj.activeSelf == false)
            {
                weaponWheelObj.SetActive(true);
            }
            else
            {
                weaponWheelObj.SetActive(false);
            }
        }
    }
    public void SelectWeapon(WeaponSlot slot)
    {
        print(slot.weaponScOb.weaponName);
    }
    public void HoverWheel(GameObject hoverRing)
    {
        
    }
}
