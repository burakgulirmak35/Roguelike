using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum GunType
{
    Rifle, Pistol, ShotGun, Sniper, Grenade, Minigun, Rocket
}

public class PlayerData : MonoBehaviour
{
    [Header("Level")]
    public List<int> expPerLevel = new List<int>();
    public int level;
    public int exp;

    [Header("DontChange")]
    public float EachBurstTime;
    public float BulletSpeed;

    [Header("Base Stats")]
    public float BaseMaxHealth;
    public float BaseMovementSpeed;
    public float BaseDamage;
    public float BaseFireRate;
    public float BaseFireRange;
    public int   BaseBurstCount;
    public int   BaseBounceCount;
    public float BaseExplosiveAmmoRange;
    public float BaseExplosiveAmmoDamage;

    [Header("Current Stats")]
    public float MaxHealth;
    public float MovementSpeed;
    public float Damage;
    public float FireRate;
    public float FireRange;
    public int   BurstCount;
    public int   BounceCount;
    public float ExplosiveAmmoRange;
    public float ExplosiveAmmoDamage;
    public bool  Penetrability;
    public bool  ExplosiveAmmo;
    public GunType SelectedGun;

    [Header("Effects")]
    [HideInInspector] public float MovementSpeedMultipler;
    [HideInInspector] public float FireRateMultipler;

    [Header("Scriptables")]
    [SerializeField][Tooltip("Script Will Set Values")] private ExplosionSO BulletExplosionSO;

    [Header("Upgrades")]
    public List<UpgradeSO> Upgrades = new List<UpgradeSO>();

    [Header("Currency")]
    [HideInInspector] public int Score;

    public static PlayerData Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        MovementSpeedMultipler = 1;
        FireRateMultipler      = 1;
    }

    void Start()
    {
        CheckUpgradesList();
    }

    // -----------------------------------------------------------------------
    // Persist & Load
    // -----------------------------------------------------------------------

    public void LoadData()
    {
        exp   = PlayerPrefs.GetInt("Exp");
        level = PlayerPrefs.GetInt("Level");
        Score = PlayerPrefs.GetInt("Score");

        MaxHealth          = PlayerPrefs.GetFloat("MaxHealth",          BaseMaxHealth);
        MovementSpeed      = PlayerPrefs.GetFloat("MovementSpeed",      BaseMovementSpeed);
        Damage             = PlayerPrefs.GetFloat("Damage",             BaseDamage);
        FireRate           = PlayerPrefs.GetFloat("FireRate",           BaseFireRate);
        FireRange          = PlayerPrefs.GetFloat("FireRange",          BaseFireRange);
        BurstCount         = PlayerPrefs.GetInt  ("BurstCount",         BaseBurstCount);
        BounceCount        = PlayerPrefs.GetInt  ("BounceCount",        BaseBounceCount);
        ExplosiveAmmoRange = PlayerPrefs.GetFloat("ExplosiveAmmoRange", BaseExplosiveAmmoRange);
        ExplosiveAmmoDamage= PlayerPrefs.GetFloat("ExplosiveAmmoDamage",BaseExplosiveAmmoDamage);
        Penetrability      = PlayerPrefs.GetInt  ("Penetrability")  == 1;
        ExplosiveAmmo      = PlayerPrefs.GetInt  ("ExplosiveAmmo")  == 1;

        BulletExplosionSO.Range  = ExplosiveAmmoRange;
        BulletExplosionSO.Damage = ExplosiveAmmoDamage;
    }

    public void SaveScore()
    {
        PlayerPrefs.SetInt("Score", Score);
        PlayerPrefs.Save();
    }

    public void ResetData()
    {
        PlayerPrefs.SetInt  ("Exp",   0);
        PlayerPrefs.SetInt  ("Level", 0);
        PlayerPrefs.SetInt  ("Score", 0);

        PlayerPrefs.SetFloat("MaxHealth",           BaseMaxHealth);
        PlayerPrefs.SetFloat("MovementSpeed",       BaseMovementSpeed);
        PlayerPrefs.SetFloat("Damage",              BaseDamage);
        PlayerPrefs.SetFloat("FireRate",            BaseFireRate);
        PlayerPrefs.SetFloat("FireRange",           BaseFireRange);
        PlayerPrefs.SetInt  ("BurstCount",          BaseBurstCount);
        PlayerPrefs.SetInt  ("BounceCount",         BaseBounceCount);
        PlayerPrefs.SetFloat("ExplosiveAmmoRange",  BaseExplosiveAmmoRange);
        PlayerPrefs.SetFloat("ExplosiveAmmoDamage", BaseExplosiveAmmoDamage);
        PlayerPrefs.SetInt  ("Penetrability",       0);
        PlayerPrefs.SetInt  ("ExplosiveAmmo",       0);

        BulletExplosionSO.Range  = BaseExplosiveAmmoRange;
        BulletExplosionSO.Damage = BaseExplosiveAmmoDamage;
    }

    // -----------------------------------------------------------------------
    // Upgrade application
    // -----------------------------------------------------------------------

    public void ApplyEffect(in UpgradeEffect e)
    {
        switch (e.stat)
        {
            case StatType.MaxHealth:
                Apply(ref MaxHealth, e);
                PlayerPrefs.SetFloat("MaxHealth", MaxHealth);
                break;
            case StatType.MovementSpeed:
                Apply(ref MovementSpeed, e);
                PlayerPrefs.SetFloat("MovementSpeed", MovementSpeed);
                break;
            case StatType.Damage:
                Apply(ref Damage, e);
                PlayerPrefs.SetFloat("Damage", Damage);
                break;
            case StatType.FireRate:
                Apply(ref FireRate, e);
                PlayerPrefs.SetFloat("FireRate", FireRate);
                break;
            case StatType.FireRange:
                Apply(ref FireRange, e);
                PlayerPrefs.SetFloat("FireRange", FireRange);
                break;
            case StatType.BurstCount:
                BurstCount += (int)e.value;
                PlayerPrefs.SetInt("BurstCount", BurstCount);
                break;
            case StatType.BounceCount:
                BounceCount += (int)e.value;
                PlayerPrefs.SetInt("BounceCount", BounceCount);
                break;
            case StatType.Penetrability:
                Penetrability = true;
                PlayerPrefs.SetInt("Penetrability", 1);
                break;
            case StatType.ExplosiveAmmo:
                ExplosiveAmmo = true;
                PlayerPrefs.SetInt("ExplosiveAmmo", 1);
                break;
            case StatType.ExplosiveAmmoRange:
                Apply(ref ExplosiveAmmoRange, e);
                BulletExplosionSO.Range = ExplosiveAmmoRange;
                PlayerPrefs.SetFloat("ExplosiveAmmoRange", ExplosiveAmmoRange);
                break;
            case StatType.ExplosiveAmmoDamage:
                Apply(ref ExplosiveAmmoDamage, e);
                BulletExplosionSO.Damage = ExplosiveAmmoDamage;
                PlayerPrefs.SetFloat("ExplosiveAmmoDamage", ExplosiveAmmoDamage);
                break;
            case StatType.HealPercent:
                Player.Instance.healthSystem.HealPercent(e.value);
                break;
        }
    }

    private static void Apply(ref float stat, in UpgradeEffect e)
    {
        switch (e.operation)
        {
            case OperationType.Add:      stat += e.value; break;
            case OperationType.Multiply: stat *= e.value; break;
            case OperationType.Set:      stat  = e.value; break;
        }
    }

    // -----------------------------------------------------------------------
    // Upgrade list — previously-unlocked tiers
    // -----------------------------------------------------------------------

    private void CheckUpgradesList()
    {
        CheckAndReplace(StatType.Penetrability, "Penetrability");
        CheckAndReplace(StatType.ExplosiveAmmo,  "ExplosiveAmmo");
    }

    private void CheckAndReplace(StatType stat, string prefKey)
    {
        if (PlayerPrefs.GetInt(prefKey) != 1) return;

        for (int i = 0; i < Upgrades.Count; i++)
        {
            if (!Upgrades[i].HasEffect(stat)) continue;

            var so = Upgrades[i];
            Upgrades.RemoveAt(i);
            if (so.nextUpgrades != null)
                Upgrades.AddRange(so.nextUpgrades);
            return;
        }
    }
}
