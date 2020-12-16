using UnityEngine;
using System.Collections;

public class AmmoPack : MonoBehaviour
{
    public string ammoType;
    public int minAmmo, maxAmmo;
    public AmmoCounter ammoScript;
    public WeaponSelector selectScript;

    public AudioSource pickUpSound;
    public float soundRange;

    private void Start()
    {
        ammoScript = GameObject.Find("WeaponManager").GetComponent<AmmoCounter>();
        selectScript = GameObject.Find("WeaponManager").GetComponent<WeaponSelector>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUpCol")
        {
            StartCoroutine(other.GetComponentInParent<UsePlayer>().PlayAudioSource(other.GetComponentInParent<UsePlayer>().sounds.itemPickUp));
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
            if (selectScript.selectedSlotScript.gunWeapon.type != "Gun")
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
    public IEnumerator PlayAudioSource(AudioSource source)
    {
        float standardVolume = source.volume;
        float standardPitch = source.pitch;
        source.volume = Random.Range(standardVolume - soundRange, standardVolume + soundRange);
        source.pitch = Random.Range(standardPitch - soundRange, standardPitch + soundRange);
        source.Play();
        yield return new WaitForSeconds(source.clip.length);
    }
}
