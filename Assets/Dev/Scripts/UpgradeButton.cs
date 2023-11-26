using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private Button btn_Upgrade;
    [SerializeField] private Image img_SkillImage;
    [SerializeField] private TextMeshProUGUI txt_SkillName;
    [SerializeField] private TextMeshProUGUI txt_SkillDescription;
    [SerializeField] private UpgradeType upgradeType;
    private PlayerData playerData;
    private Player player;

    void Awake()
    {
        playerData = FindObjectOfType<PlayerData>();
        player = FindObjectOfType<Player>();
        btn_Upgrade.onClick.AddListener(BtnUpgrade);
    }

    public void SetUpgradeSO(UpgradeSO upgradeSO)
    {
        img_SkillImage.sprite = upgradeSO.sprite_SkillImage;
        txt_SkillName.text = upgradeSO.string_SkillName;
        txt_SkillDescription.text = upgradeSO.string_SkillDescription;
        upgradeType = upgradeSO.upgradeType;
    }

    private void BtnUpgrade()
    {
        switch (upgradeType)
        {
            case UpgradeType.SmallHeal:
                player.healthSystem.HealPercent(PlayerData.Instance.SmallHealPercent);
                break;
            case UpgradeType.MediumHeal:
                player.healthSystem.HealPercent(PlayerData.Instance.MediumHealPercent);
                break;
            case UpgradeType.LargeHeal:
                player.healthSystem.HealPercent(PlayerData.Instance.LargeHealPercent);
                break;
            case UpgradeType.AddMaxHealth:
                PlayerPrefs.SetFloat("AddedMaxHealth", PlayerPrefs.GetFloat("AddedMaxHealth") + (PlayerPrefs.GetFloat("AddedMaxHealth") * PlayerData.Instance.AddMaxHealthPercent));
                break;
            case UpgradeType.AddMovementSpeed:
                PlayerPrefs.SetFloat("AddedMovementSpeed", PlayerPrefs.GetFloat("AddedMovementSpeed") + PlayerData.Instance.AddMovementSpeed);
                break;
            case UpgradeType.AddDamage:
                PlayerPrefs.SetInt("AddedDamage", PlayerPrefs.GetInt("AddedDamage") + PlayerData.Instance.AddDamage);
                break;
            case UpgradeType.AddFireRate:
                PlayerPrefs.SetFloat("AddedFireRate", PlayerPrefs.GetFloat("AddedFireRate") + PlayerData.Instance.AddedFireRateAmount);
                break;
            case UpgradeType.AddFireRange:
                PlayerPrefs.SetFloat("AddedFireRange", PlayerPrefs.GetFloat("AddedFireRange") + PlayerData.Instance.AddedFireRangeAmount);
                break;
            case UpgradeType.AddBurstCount:
                PlayerPrefs.SetInt("AddedBurstCount", PlayerPrefs.GetInt("AddedBurstCount") + 1);
                break;
            case UpgradeType.AddBounceCount:
                PlayerPrefs.SetInt("AddedBounceCount", PlayerPrefs.GetInt("AddedBounceCount") + 1);
                break;
            case UpgradeType.Penetrability:
                PlayerPrefs.SetInt("Penetrability", 1);
                break;
            case UpgradeType.ExplosiveAmmo:
                PlayerPrefs.SetInt("ExplosiveAmmo", 1);
                break;
            case UpgradeType.ExplosiveAmmoRange:
                PlayerPrefs.SetFloat("AddedExplosiveAmmoRange", PlayerData.Instance.AddExplosiveAmmoRange);
                break;
            case UpgradeType.ExplosiveAmmoDamage:
                PlayerPrefs.SetInt("AddedExplosiveAmmoDamage", PlayerData.Instance.AddExplosiveAmmoDamage);
                break;
        }
        UIManager.Instance.EnablePanelUpgrade(false);
        playerData.LoadData();
        player.CheckUpgrades();
    }


}
