using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelector : MonoBehaviour
{
    public GameObject weaponWheelObj;
    public GameObject[] rings;

    public Slot[] selectedSlots;
    [System.Serializable]
    public class Colors
    {
        public ColorBlock selectedColor, normalColor;
    }
    public Colors colors;

    private void Start()
    {
        HoverWheel(weaponWheelObj.transform.GetChild(0).gameObject);
    }
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
    public void SelectWeapon(Slot slot)
    {
        if(slot.weaponScOb != null)
        {
            if(slot.slotType == "Gun")
            {
                selectedSlots[0] = slot;
            }
            else if(slot.slotType == "Melee")
            {
                selectedSlots[1] = slot;
            }
            else if(slot.slotType == "Trowable")
            {
                selectedSlots[2] = slot;
            }
            print(slot.weaponScOb.weaponName);
        }
    }
    public void HoverWheel(GameObject hoverRing)
    {
        foreach(GameObject g in rings)
        {
            if (g != hoverRing)
            {
                foreach (Transform child in g.transform)
                {
                    child.GetComponent<Button>().colors = colors.normalColor;
                }
            }
        }
        foreach(Transform child in hoverRing.transform)
        {
            child.GetComponent<Button>().colors = colors.selectedColor;
        }
    }
}
