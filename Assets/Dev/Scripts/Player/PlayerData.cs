using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // PlayerPrefs key sabitleri — tek yerden yönetilir, typo riski yok
    // -----------------------------------------------------------------------
    private static class Keys
    {
        public const string Exp = "Exp";
        public const string Level = "Level";
        public const string Score = "Score";
        public const string MaxHealth = "MaxHealth";
        public const string MovementSpeed = "MovementSpeed";
        public const string Damage = "Damage";
        public const string FireRate = "FireRate";
        public const string FireRange = "FireRange";
        public const string BurstCount = "BurstCount";
        public const string BounceCount = "BounceCount";
        public const string ExplosiveAmmoRange = "ExplosiveAmmoRange";
        public const string ExplosiveAmmoDamage = "ExplosiveAmmoDamage";
        public const string Penetrability = "Penetrability";
        public const string ExplosiveAmmo = "ExplosiveAmmo";
    }

    // -----------------------------------------------------------------------
    // Inspector alanları
    // -----------------------------------------------------------------------

    [Header("Level")]
    public List<int> expToLevelUp = new List<int>();
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
    public int BaseBurstCount;
    public int BaseBounceCount;
    public float BaseExplosiveAmmoRange;
    public float BaseExplosiveAmmoDamage;

    [Header("Current Stats")]
    public float MaxHealth;
    public float MovementSpeed;
    public float Damage;
    public float FireRate;
    public float FireRange;
    public int BurstCount;
    public int BounceCount;
    public float ExplosiveAmmoRange;
    public float ExplosiveAmmoDamage;
    public bool Penetrability;
    public bool ExplosiveAmmo;

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
        FireRateMultipler = 1;
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
        exp = PlayerPrefs.GetInt(Keys.Exp);
        level = PlayerPrefs.GetInt(Keys.Level);
        Score = PlayerPrefs.GetInt(Keys.Score);

        MaxHealth = PlayerPrefs.GetFloat(Keys.MaxHealth, BaseMaxHealth);
        MovementSpeed = PlayerPrefs.GetFloat(Keys.MovementSpeed, BaseMovementSpeed);
        Damage = PlayerPrefs.GetFloat(Keys.Damage, BaseDamage);
        FireRate = PlayerPrefs.GetFloat(Keys.FireRate, BaseFireRate);
        FireRange = PlayerPrefs.GetFloat(Keys.FireRange, BaseFireRange);
        BurstCount = PlayerPrefs.GetInt(Keys.BurstCount, BaseBurstCount);
        BounceCount = PlayerPrefs.GetInt(Keys.BounceCount, BaseBounceCount);
        ExplosiveAmmoRange = PlayerPrefs.GetFloat(Keys.ExplosiveAmmoRange, BaseExplosiveAmmoRange);
        ExplosiveAmmoDamage = PlayerPrefs.GetFloat(Keys.ExplosiveAmmoDamage, BaseExplosiveAmmoDamage);
        Penetrability = PlayerPrefs.GetInt(Keys.Penetrability) == 1;
        ExplosiveAmmo = PlayerPrefs.GetInt(Keys.ExplosiveAmmo) == 1;

        BulletExplosionSO.Range = ExplosiveAmmoRange;
        BulletExplosionSO.Damage = ExplosiveAmmoDamage;
    }

    public void SaveScore()
    {
        PlayerPrefs.SetInt(Keys.Score, Score);
        PlayerPrefs.Save();
    }

    public void ResetData()
    {
        PlayerPrefs.SetInt(Keys.Exp, 0);
        PlayerPrefs.SetInt(Keys.Level, 0);
        PlayerPrefs.SetInt(Keys.Score, 0);
        PlayerPrefs.SetFloat(Keys.MaxHealth, BaseMaxHealth);
        PlayerPrefs.SetFloat(Keys.MovementSpeed, BaseMovementSpeed);
        PlayerPrefs.SetFloat(Keys.Damage, BaseDamage);
        PlayerPrefs.SetFloat(Keys.FireRate, BaseFireRate);
        PlayerPrefs.SetFloat(Keys.FireRange, BaseFireRange);
        PlayerPrefs.SetInt(Keys.BurstCount, BaseBurstCount);
        PlayerPrefs.SetInt(Keys.BounceCount, BaseBounceCount);
        PlayerPrefs.SetFloat(Keys.ExplosiveAmmoRange, BaseExplosiveAmmoRange);
        PlayerPrefs.SetFloat(Keys.ExplosiveAmmoDamage, BaseExplosiveAmmoDamage);
        PlayerPrefs.SetInt(Keys.Penetrability, 0);
        PlayerPrefs.SetInt(Keys.ExplosiveAmmo, 0);
        PlayerPrefs.Save();

        BulletExplosionSO.Range = BaseExplosiveAmmoRange;
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
                PlayerPrefs.SetFloat(Keys.MaxHealth, MaxHealth);
                break;
            case StatType.MovementSpeed:
                Apply(ref MovementSpeed, e);
                PlayerPrefs.SetFloat(Keys.MovementSpeed, MovementSpeed);
                break;
            case StatType.Damage:
                Apply(ref Damage, e);
                PlayerPrefs.SetFloat(Keys.Damage, Damage);
                break;
            case StatType.FireRate:
                Apply(ref FireRate, e);
                PlayerPrefs.SetFloat(Keys.FireRate, FireRate);
                break;
            case StatType.FireRange:
                Apply(ref FireRange, e);
                PlayerPrefs.SetFloat(Keys.FireRange, FireRange);
                break;
            case StatType.BurstCount:
                ApplyInt(ref BurstCount, e);
                PlayerPrefs.SetInt(Keys.BurstCount, BurstCount);
                break;
            case StatType.BounceCount:
                ApplyInt(ref BounceCount, e);
                PlayerPrefs.SetInt(Keys.BounceCount, BounceCount);
                break;
            case StatType.Penetrability:
                Penetrability = true;
                PlayerPrefs.SetInt(Keys.Penetrability, 1);
                break;
            case StatType.ExplosiveAmmo:
                ExplosiveAmmo = true;
                PlayerPrefs.SetInt(Keys.ExplosiveAmmo, 1);
                break;
            case StatType.ExplosiveAmmoRange:
                Apply(ref ExplosiveAmmoRange, e);
                BulletExplosionSO.Range = ExplosiveAmmoRange;
                PlayerPrefs.SetFloat(Keys.ExplosiveAmmoRange, ExplosiveAmmoRange);
                break;
            case StatType.ExplosiveAmmoDamage:
                Apply(ref ExplosiveAmmoDamage, e);
                BulletExplosionSO.Damage = ExplosiveAmmoDamage;
                PlayerPrefs.SetFloat(Keys.ExplosiveAmmoDamage, ExplosiveAmmoDamage);
                break;
            case StatType.HealPercent:
                GameEvents.RaiseHealRequest(e.value);
                break;
        }

        PlayerPrefs.Save();
    }

    private static void Apply(ref float stat, in UpgradeEffect e)
    {
        switch (e.operation)
        {
            case OperationType.Add: stat += e.value; break;
            case OperationType.Multiply: stat *= e.value; break;
            case OperationType.Set: stat = e.value; break;
        }
    }

    private static void ApplyInt(ref int stat, in UpgradeEffect e)
    {
        switch (e.operation)
        {
            case OperationType.Add: stat += (int)e.value; break;
            case OperationType.Multiply: stat = Mathf.RoundToInt(stat * e.value); break;
            case OperationType.Set: stat = (int)e.value; break;
        }
    }

    // -----------------------------------------------------------------------
    // Upgrade list — previously-unlocked tiers
    // -----------------------------------------------------------------------

    private void CheckUpgradesList()
    {
        CheckAndReplace(StatType.Penetrability, Keys.Penetrability);
        CheckAndReplace(StatType.ExplosiveAmmo, Keys.ExplosiveAmmo);
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
