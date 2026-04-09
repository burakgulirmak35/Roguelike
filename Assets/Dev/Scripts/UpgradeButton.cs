using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private Button          btn_Upgrade;
    [SerializeField] private Image           img_Icon;
    [SerializeField] private TextMeshProUGUI txt_Name;
    [SerializeField] private TextMeshProUGUI txt_Description;

    private UpgradeSO _so;

    void Awake() => btn_Upgrade.onClick.AddListener(OnClick);

    public void SetUpgradeSO(UpgradeSO so)
    {
        _so                  = so;
        img_Icon.sprite      = so.icon;
        txt_Name.text        = so.skillName;
        txt_Description.text = so.description;
    }

    private void OnClick()
    {
        var effects = _so.effects;
        for (int i = 0; i < effects.Length; i++)
            PlayerData.Instance.ApplyEffect(effects[i]);

        PlayerData.Instance.Upgrades.Remove(_so);
        if (_so.nextUpgrades != null)
            PlayerData.Instance.Upgrades.AddRange(_so.nextUpgrades);

        UIManager.Instance.EnablePanelUpgrade(false);
        Player.Instance.CheckUpgrades();
    }
}
