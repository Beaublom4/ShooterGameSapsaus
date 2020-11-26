using UnityEngine;

public class AmmoPack : MonoBehaviour
{
    public string ammoType;
    public int minAmmo, maxAmmo;
    public AmmoCounter ammoScript;

    private void Start()
    {
        ammoScript = GameObject.Find("WeaponManager").GetComponent<AmmoCounter>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUpCol")
        {
            if (ammoType == "Pistol")
            {
                int ammo = Random.Range(minAmmo, maxAmmo);
                ammoScript.pistolAmmo += ammo;
                ammoScript.UpdatePistolAmmoLeft();
            }
            else if (ammoType == "Sniper")
            {
                int ammo = Random.Range(minAmmo, maxAmmo);
                ammoScript.sniperAmmo += ammo;
                ammoScript.UpdateSniperAmmoLeft();
            }
            else if ( ammoType == "Shotgun")
            {
                int ammo = Random.Range(minAmmo, maxAmmo);
                ammoScript.shotgunAmmo += ammo;
                ammoScript.UpdateShotgunAmmoLeft();
            }
            Destroy(gameObject);
        }
    }
}
