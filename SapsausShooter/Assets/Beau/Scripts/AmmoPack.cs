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
        if (other.gameObject.tag == "Player")
        {
            if (ammoType == "Pistol")
            {
                int ammo = Random.Range(minAmmo, maxAmmo);
                ammoScript.pistolAmmo += ammo;
                ammoScript.UpdatePistolAmmoLeft();
                Destroy(gameObject);
            }
        }
    }
}
