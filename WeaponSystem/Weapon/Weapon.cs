using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    public Sprite weaponSprite;
    public float fireRate;
    public float distance;
    public int damage;
    public AmmoType ammoType;
    public int currentAmmo;
    public int maxAmmo;
    public Vector3 defaultPosition;

    public string WeaponName => weaponName;
    public Sprite WeaponSprite => weaponSprite;
    public float FireRate => fireRate;
    public float Distance => distance;
    public int Damage => damage;
    public AmmoType AmmoType => ammoType;
    public int CurrentAmmo => currentAmmo;
    public int MaxAmmo => maxAmmo;
    public Vector3 DefaultPosition => defaultPosition;
}

public enum AmmoType
{
    Rifle,
    SMG,
    Pistol,
    Shotgun,
    RPG,
    Snipe,
    Melee
}