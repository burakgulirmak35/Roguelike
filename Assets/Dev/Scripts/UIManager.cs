using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Controllers")]
    [SerializeField] private GameObject LeftJoystick;
    [SerializeField] private GameObject RightJoystick;
    [Header("Buttons")]
    [SerializeField] private GameObject img_ToggleAim;
    [SerializeField] private GameObject img_AutoAim;
    [SerializeField] private GameObject img_ManuelAim;
    [Header("Panels")]
    [SerializeField] public PanelPlayerDead panelPlayerDead;
    [SerializeField] public PanelUpgrade panelUpgrade;
    [SerializeField] public PanelFadeInOut panelFadeInOut;

    void Awake()
    {
        Instance = this;
        panelFadeInOut.FadeIn(1f);
    }

    void Start()
    {
        CloseAllPanels();
    }

    private void CloseAllPanels()
    {
        EnablePanelUpgrade(false);
        EnablePanelPlayerDead(false);
    }

    public void EnablePanelPlayerDead(bool _state)
    {
        if (_state)
        {
            CameraManager.Instance.ZoomTo(0);
        }

        panelPlayerDead.gameObject.SetActive(_state);
    }

    public void EnablePanelUpgrade(bool _state)
    {
        if (_state)
        {
            panelUpgrade.SetUpgradeButtons();
            panelUpgrade.gameObject.SetActive(true);

            GameManager.Instance.FreezeGame();
        }
        else
        {
            panelUpgrade.gameObject.SetActive(false);

            GameManager.Instance.ResumeGame();
        }
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
}
