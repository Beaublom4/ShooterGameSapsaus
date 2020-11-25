using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Gun weapon;
    public float ammoInMag;
    public Image image;

    private void OnEnable()
    {
        if(weapon != null)
        {
            image.sprite = weapon.uiSprite;
        }
        else
        {
            image.sprite = null;
        }
    }
}
