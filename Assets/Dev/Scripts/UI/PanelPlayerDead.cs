using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelPlayerDead : MonoBehaviour
{
    [SerializeField] private Button btn_MainMenu;
    [SerializeField] private Button btn_TryAgain;
    [SerializeField] private Button btn_Continue;

    void Awake()
    {
        btn_MainMenu.onClick.AddListener(GameManager.Instance.MainMenu);
        btn_TryAgain.onClick.AddListener(GameManager.Instance.RestartGame);
        btn_Continue.onClick.AddListener(GameManager.Instance.ContinueGame);
    }

}
