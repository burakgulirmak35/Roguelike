using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Roguelike/UpgradeSO", order = 0)]
public class UpgradeSO : ScriptableObject
{
    public UpgradeType upgradeType;
    public Sprite sprite_SkillImage;
    public string string_SkillName;
    public string string_SkillDescription;
}

public enum UpgradeType
{
    AddMaxHealth, AddMovementSpeed, AddDamage, AddFireRate, AddFireRange, AddBurstCount, AddBounceCount, Penetrability, ExplosiveAmmo, ExplosiveAmmoRange, ExplosiveAmmoDamage,
    SmallHeal, MediumHeal, LargeHeal
}