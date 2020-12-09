using UnityEngine;

public class AmmoPack : MonoBehaviour
{
    public string ammoType;
    public int minAmmo, maxAmmo;
    public AmmoCounter ammoScript;
    public WeaponSelector selectScript;

    private void Start()
    {
        ammoScript = GameObject.Find("WeaponManager").GetComponent<AmmoCounter>();
        selectScript = GameObject.Find("WeaponManager").GetComponent<WeaponSelector>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUpCol")
        {
            other.GetComponentInParent<UsePlayer>().PlayAudioSource(other.GetComponentInParent<UsePlayer>().sounds.itemPickUp);
            if (ammoType == "Pistol")
            {
                int ammo = Random.Range(minAmmo, maxAmmo);
                ammoScript.pistolAmmo += ammo;
            }
            else if (ammoType == "Sniper")
            {
                int ammo = Random.Range(minAmmo, maxAmmo);
                ammoScript.sniperAmmo += ammo;
            }
            else if ( ammoType == "Shotgun")
            {
                int ammo = Random.Range(minAmmo, maxAmmo);
                ammoScript.shotgunAmmo += ammo;
            }
            else if (ammoType == "Launcher")
            {
                int ammo = Random.Range(minAmmo, maxAmmo);
                ammoScript.launcherAmmo += ammo;
            }
            else if (ammoType == "Special")
            {
                int ammo = Random.Range(minAmmo, maxAmmo);
                ammoScript.specialAmmo += ammo;
            }

            if (selectScript.selectedSlotScript.gunWeapon = null)
            {
                Destroy(gameObject);
            }
            if (selectScript.selectedSlotScript.gunWeapon.gunType == "Pistol")
            {
                ammoScript.UpdatePistolAmmoLeft();
            }
            else if(selectScript.selectedSlotScript.gunWeapon.gunType == "Sniper")
            {
                ammoScript.UpdateSniperAmmoLeft();
            }
            else if (selectScript.selectedSlotScript.gunWeapon.gunType == "Shotgun")
            {
                ammoScript.UpdateShotgunAmmoLeft();
            }
            else if (selectScript.selectedSlotScript.gunWeapon.gunType == "Launcher")
            {
                ammoScript.UpdateShotgunAmmoLeft();
            }
            else if (selectScript.selectedSlotScript.gunWeapon.gunType == "Special")
            {
                ammoScript.UpdateShotgunAmmoLeft();
            }
            Destroy(gameObject);
        }
    }
}
