using UnityEngine;

public enum StatType
{
    MaxHealth, MovementSpeed,
    Damage, FireRate, FireRange, BurstCount, BounceCount,
    Penetrability, ExplosiveAmmo, ExplosiveAmmoRange, ExplosiveAmmoDamage,
    HealPercent
}

public enum OperationType { Add, Multiply, Set }

public enum UpgradeRarity { Common, Uncommon, Rare, Epic, Legendary }

[System.Serializable]
public struct UpgradeEffect
{
    public StatType      stat;
    public OperationType operation;
    public float         value;
}

[CreateAssetMenu(fileName = "Upgrade", menuName = "Roguelike/UpgradeSO")]
public class UpgradeSO : ScriptableObject
{
    public string        skillName;
    public string        description;
    public Sprite        icon;
    public UpgradeRarity rarity;
    public UpgradeEffect[] effects;
    public UpgradeSO[]   nextUpgrades;

    /// <summary>Bu SO'nun verilen stat'ı etkileyen bir effect'i var mı?</summary>
    public bool HasEffect(StatType stat)
    {
        for (int i = 0; i < effects.Length; i++)
            if (effects[i].stat == stat) return true;
        return false;
    }
}
