using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelUpgrade : MonoBehaviour
{
    [SerializeField] private List<UpgradeButton> upgradeButtons = new List<UpgradeButton>();
    private UpgradeSO upgradeSO;

    public void SetUpgradeButtons()
    {
        for (int i = 0; i < upgradeButtons.Count; i++)
        {
            upgradeSO = PlayerData.Instance.Upgrades[Random.Range(0, PlayerData.Instance.Upgrades.Count)];
            upgradeButtons[i].SetUpgradeSO(upgradeSO);
        }
    }

}
