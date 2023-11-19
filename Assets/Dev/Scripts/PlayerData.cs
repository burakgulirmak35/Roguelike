using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum GunType
{
    Rifle, Pistol, ShotGun, Sniper, Grenade, Minigun, Rocket
}

public class PlayerData : MonoBehaviour
{
    [Header("LevelSettings")]
    public List<int> expPerLevel = new List<int>();
    public List<UpgradeSO> Upgrades = new List<UpgradeSO>();

    [Header("DontChange")]
    public float EachBurstTime;
    public float BulletSpeed;
    [Header("Base")]
    public float BaseMaxHealth;
    public float BaseMovementSpeed;
    [Space]
    public float BaseDamage;
    public float BaseFireRate;
    public float BaseFireRange;
    public int BaseBurstCount;
    public int BaseBounceCount;
    [Space]
    public float BaseExplosiveAmmoRange;
    public float BaseExplosiveAmmoDamage;
    [Header("Current")]
    public float MaxHealth;
    public float MovementSpeed;
    [Space]
    public float Damage;
    public float FireRate;
    public float FireRange;
    public int BurstCount;
    public int BounceCount;
    public float ExplosiveAmmoRange;
    public float ExplosiveAmmoDamage;
    [Header("Special")]
    public bool Penetrability;
    public bool ExplosiveAmmo;
    public GunType SelectedGun;
    [Header("Level")]
    public int level;
    public int exp;

    public static PlayerData Instance { get; private set; }
    void Awake()
    {
        Instance = this;
    }

    public void LoadData()
    {
        exp = PlayerPrefs.GetInt("Exp");
        level = PlayerPrefs.GetInt("Level");

        MaxHealth = BaseMaxHealth + PlayerPrefs.GetInt("AddedMaxHealth");
        MovementSpeed = BaseMovementSpeed + PlayerPrefs.GetFloat("AddedMovementSpeed");
        Damage = BaseDamage + PlayerPrefs.GetInt("AddedDamage");
        FireRate = BaseFireRate + PlayerPrefs.GetFloat("AddedFireRate");
        FireRange = BaseFireRange + PlayerPrefs.GetFloat("AddedFireRange");
        BurstCount = BaseBurstCount + PlayerPrefs.GetInt("AddedBurstCount");
        BounceCount = BaseBounceCount + PlayerPrefs.GetInt("AddedBounceCount");
        ExplosiveAmmoRange = BaseExplosiveAmmoRange + PlayerPrefs.GetInt("AddedExplosiveAmmoRange");
        ExplosiveAmmoDamage = BaseExplosiveAmmoDamage + PlayerPrefs.GetInt("AddedExplosiveAmmoDamage");

        Penetrability = PlayerPrefs.GetInt("Penetrability") == 1 ? true : false;
        ExplosiveAmmo = PlayerPrefs.GetInt("ExplosiveAmmo") == 1 ? true : false;
    }
}
