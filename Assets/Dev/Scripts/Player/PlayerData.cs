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
    [Header("------------LevelSettings------------")]
    public List<int> expPerLevel = new List<int>();
    [Header("------------Level------------")]
    public int level;
    public int exp;

    [Header("DontChange")]
    public float EachBurstTime;
    public float BulletSpeed;
    [Header("------------Base------------")]
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
    [Header("------------Current------------")]
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
    [Space]
    public bool Penetrability;
    public bool ExplosiveAmmo;
    public GunType SelectedGun;

    [Header("------------UpgradeSettings------------")]
    public List<UpgradeSO> Upgrades = new List<UpgradeSO>();
    [Header("------------------------")]
    [Range(0, 1)] public float SmallHealPercent;
    [Range(0, 1)] public float MediumHealPercent;
    [Range(0, 1)] public float LargeHealPercent;
    [Range(0, 1)] public float AddMaxHealthPercent;
    [Range(0, 1)] public float AddMovementSpeed;
    [Range(0, 100)] public int AddDamage;
    [Range(0, 1)] public float AddedFireRateAmount;
    [Range(0, 1)] public float AddedFireRangeAmount;
    [Range(0, 100)] public float AddExplosiveAmmoRange;
    [Range(0, 100)] public int AddExplosiveAmmoDamage;

    [Header("------------Effects------------")]
    [HideInInspector] public float MovementSpeedMultipler;
    [HideInInspector] public float FireRateMultipler;

    [Header("------------Scriptables------------")]
    [SerializeField][Tooltip("Script Will Set Values")] private ExplosionSO BulletExplosionSO;

    [Header("------------Currency------------")]
    [HideInInspector] public int Score;

    public static PlayerData Instance { get; private set; }
    void Awake()
    {
        Instance = this;

        MovementSpeedMultipler = 1;
        FireRateMultipler = 1;
    }

    void Start()
    {
        CheckUpgradesList();
        CheckUpgradesList();
    }

    private UpgradeSO tempUpgradeSO;
    private void CheckUpgradesList()
    {
        if (PlayerPrefs.GetInt("Penetrability") == 1)
        {
            for (int i = 0; i < Upgrades.Count; i++)
            {
                if (Upgrades[i].upgradeType.Equals(UpgradeType.Penetrability))
                {
                    tempUpgradeSO = Upgrades[i];
                    Upgrades.RemoveAt(i);
                    Upgrades.Remove(tempUpgradeSO);
                    Upgrades.AddRange(tempUpgradeSO.NextUpgrades);
                    break;
                }
            }
        }

        if (PlayerPrefs.GetInt("ExplosiveAmmo") == 1)
        {
            for (int i = 0; i < Upgrades.Count; i++)
            {
                if (Upgrades[i].upgradeType.Equals(UpgradeType.ExplosiveAmmo))
                {
                    tempUpgradeSO = Upgrades[i];
                    Upgrades.RemoveAt(i);
                    Upgrades.Remove(tempUpgradeSO);
                    Upgrades.AddRange(tempUpgradeSO.NextUpgrades);
                    break;
                }
            }
        }
    }

    public void LoadData()
    {
        exp = PlayerPrefs.GetInt("Exp");
        level = PlayerPrefs.GetInt("Level");
        Score = PlayerPrefs.GetInt("Score");

        MaxHealth = BaseMaxHealth + PlayerPrefs.GetFloat("AddedMaxHealth");
        MovementSpeed = BaseMovementSpeed + PlayerPrefs.GetFloat("AddedMovementSpeed");
        Damage = BaseDamage + PlayerPrefs.GetInt("AddedDamage");
        FireRate = BaseFireRate + PlayerPrefs.GetFloat("AddedFireRate");
        FireRange = BaseFireRange + PlayerPrefs.GetFloat("AddedFireRange");
        BurstCount = BaseBurstCount + PlayerPrefs.GetInt("AddedBurstCount");
        BounceCount = BaseBounceCount + PlayerPrefs.GetInt("AddedBounceCount");
        ExplosiveAmmoRange = BaseExplosiveAmmoRange + PlayerPrefs.GetFloat("AddedExplosiveAmmoRange");
        ExplosiveAmmoDamage = BaseExplosiveAmmoDamage + PlayerPrefs.GetFloat("AddedExplosiveAmmoDamage");

        Penetrability = PlayerPrefs.GetInt("Penetrability") == 1 ? true : false;
        ExplosiveAmmo = PlayerPrefs.GetInt("ExplosiveAmmo") == 1 ? true : false;

        BulletExplosionSO.Range = ExplosiveAmmoRange;
        BulletExplosionSO.Damage = ExplosiveAmmoDamage;
    }

    public void ResetData()
    {
        PlayerPrefs.SetInt("Exp", 0);
        PlayerPrefs.SetInt("Level", 0);
        PlayerPrefs.SetInt("Score", 0);

        PlayerPrefs.SetFloat("AddedMaxHealth", 0);
        PlayerPrefs.SetFloat("AddedMovementSpeed", 0);
        PlayerPrefs.SetInt("AddedDamage", 0);
        PlayerPrefs.SetFloat("AddedFireRate", 0);
        PlayerPrefs.SetFloat("AddedFireRange", 0);
        PlayerPrefs.SetInt("AddedBurstCount", 0);
        PlayerPrefs.SetInt("AddedBounceCount", 0);
        PlayerPrefs.SetFloat("AddedExplosiveAmmoRange", 0);
        PlayerPrefs.SetFloat("AddedExplosiveAmmoDamage", 0);

        PlayerPrefs.SetInt("Penetrability", 0);
        PlayerPrefs.SetInt("ExplosiveAmmo", 0);
        // PlayerPrefs.SetInt("AutoAim", 1);

        BulletExplosionSO.Range = ExplosiveAmmoRange;
        BulletExplosionSO.Damage = ExplosiveAmmoDamage;
    }
}
