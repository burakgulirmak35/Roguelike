using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject PanelUpgrade;
    [SerializeField] private List<UpgradeButton> upgradeButtons = new List<UpgradeButton>();
    private UpgradeSO upgradeSO;
    [Header("Controllers")]
    [SerializeField] private GameObject LeftJoystick;
    [SerializeField] private GameObject RightJoystick;
    [Header("Buttons")]
    [SerializeField] private GameObject img_ToggleAim;
    [SerializeField] private GameObject img_AutoAim;
    [SerializeField] private GameObject img_ManuelAim;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CloseAllPanels();
    }

    public void ToggleAim()
    {
        if (Player.Instance.isAutoAim)
        {
            RightJoystick.SetActive(false);

            img_AutoAim.SetActive(true);
            img_ManuelAim.SetActive(false);
        }
        else
        {
            RightJoystick.SetActive(true);

            img_AutoAim.SetActive(false);
            img_ManuelAim.SetActive(true);
        }
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
                upgradeSO = PlayerData.Instance.Upgrades[Random.Range(0, PlayerData.Instance.Upgrades.Count)];
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
