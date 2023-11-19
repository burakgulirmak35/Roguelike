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
    [Header("Controllers")]
    [SerializeField] private GameObject AimJoystick;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CloseAllPanels();
    }

    private void CheckOrientation()
    {
        if (Screen.orientation == ScreenOrientation.Portrait)
        {
            AimJoystick.SetActive(false);
            Player.Instance.EnableAutoAim(false);
        }
        else
        {
            AimJoystick.SetActive(true);
            Player.Instance.EnableAutoAim(true);
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
