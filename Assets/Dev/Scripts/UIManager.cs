using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject PanelUpgrade;
    [SerializeField] private List<UpgradeButton> upgradeButtons = new List<UpgradeButton>();
    [SerializeField] private List<Upgrade> upgrades = new List<Upgrade>();
    private Upgrade _upgrade;

    void Awake()
    {
        Instance = this;
    }

    public void EnablePanelUpgrade(bool _state)
    {
        if (_state)
        {
            for (int i = 0; i < upgradeButtons.Count; i++)
            {
                _upgrade = upgrades[Random.Range(0, upgrades.Count)];
                //  upgradeButtons[i].
            }

            PanelUpgrade.SetActive(true);
        }
        else
        {
            PanelUpgrade.SetActive(false);
        }
    }
}
