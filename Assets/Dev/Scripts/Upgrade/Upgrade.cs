using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Roguelike/Upgrade", order = 0)]
public class Upgrade : ScriptableObject
{
    public UpgradeType upgradeType;
    public Sprite sprite_SkillImage;
    public string txt_SkillName;
    public string txt_SkillDescription;
}

public enum UpgradeType
{
    CorpseExplosion,
}