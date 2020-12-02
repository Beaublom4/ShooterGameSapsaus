using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Gun gunWeapon;
    public Melee meleeWeapon;
    public float ammoInMag;
    public Image image;
    public Sprite normalSprite;

    private void OnEnable()
    {
        if(gunWeapon != null)
        {
            image.sprite = gunWeapon.uiSprite;
        }
        else if(meleeWeapon != null)
        {
            image.sprite = meleeWeapon.uiSprite;
        }
        else
        {
            image.sprite = normalSprite;
        }
    }
}
