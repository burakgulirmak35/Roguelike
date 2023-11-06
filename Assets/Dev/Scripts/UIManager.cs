using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject PanelUpgrade;
    [SerializeField] private List<UpgradeButton> upgradeButtons = new List<UpgradeButton>();
    private UpgradeSO upgradeSO;
    private PlayerData playerData;

    void Awake()
    {
        Instance = this;
        playerData = FindObjectOfType<PlayerData>();
    }

    void Start()
    {
        CloseAllPanels();
    }

    private void CloseAllPanels()
    {
        EnablePanelUpgrade(false);
    }

    public void EnablePanelUpgrade(bool _state)
    {
        if (_state)
        {
            for (int i = 0; i < upgradeButtons.Count; i++)
            {
                upgradeSO = playerData.Upgrades[Random.Range(0, playerData.Upgrades.Count)];
                upgradeButtons[i].SetUpgradeSO(upgradeSO);
            }
            PanelUpgrade.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            PanelUpgrade.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
