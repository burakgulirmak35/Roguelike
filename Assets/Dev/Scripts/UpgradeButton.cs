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

    void Awake()
    {
        playerData = FindObjectOfType<PlayerData>();
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
                Player.Instance.healthSystem.HealPercent(0.25f);
                break;
            case UpgradeType.MediumHeal:
                Player.Instance.healthSystem.HealPercent(0.5f);
                break;
            case UpgradeType.LargeHeal:
                Player.Instance.healthSystem.HealPercent(1f);
                break;
            case UpgradeType.AddMaxHealth:
                PlayerPrefs.SetInt("AddedMaxHealth", PlayerPrefs.GetInt("AddedMaxHealth") + 25);
                break;
            case UpgradeType.AddMovementSpeed:
                PlayerPrefs.SetFloat("AddedMovementSpeed", PlayerPrefs.GetFloat("AddedMovementSpeed") + 0.25f);
                break;
            case UpgradeType.AddDamage:
                PlayerPrefs.SetInt("AddedDamage", PlayerPrefs.GetInt("AddedDamage") + 5);
                break;
            case UpgradeType.AddFireRate:
                PlayerPrefs.SetFloat("AddedFireRate", PlayerPrefs.GetFloat("AddedFireRate") + 0.25f);
                break;
            case UpgradeType.AddFireRange:
                PlayerPrefs.SetFloat("AddedFireRange", PlayerPrefs.GetFloat("AddedFireRange") + 1);
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
                PlayerPrefs.SetInt("AddedExplosiveAmmoRange", 1);
                break;
            case UpgradeType.ExplosiveAmmoDamage:
                PlayerPrefs.SetInt("AddedExplosiveAmmoDamage", 1);
                break;
        }
        UIManager.Instance.EnablePanelUpgrade(false);
        playerData.LoadData();
        Player.Instance.CheckUpgrades();
    }


}
