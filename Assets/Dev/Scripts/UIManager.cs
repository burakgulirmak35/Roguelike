using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Controllers")]
    [SerializeField] public Transform LeftJoystick;
    [SerializeField] private GameObject RightJoystick;
    private Vector2 LeftJoystickBasePos;
    [Header("Buttons")]
    [SerializeField] private GameObject img_ToggleAim;
    [SerializeField] private GameObject img_AutoAim;
    [SerializeField] private GameObject img_ManuelAim;
    [Space]
    [SerializeField] private Button btn_ToggleMusic;
    [SerializeField] public GameObject img_MusicOn;
    [SerializeField] public GameObject img_MusicOff;
    [Header("Panels")]
    [SerializeField] public PanelPlayerDead panelPlayerDead;
    [SerializeField] public PanelUpgrade panelUpgrade;
    [SerializeField] public PanelFadeInOut panelFadeInOut;
    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI txt_Score;
    private Transform txt_ScoreTransform;

    [Header("DG.T")]
    private Sequence mySequence;


    private void Awake()
    {
        Instance = this;
        panelFadeInOut.FadeIn(1f);
        LeftJoystickBasePos = LeftJoystick.position;

        txt_ScoreTransform = txt_Score.transform;
    }

    private void Start()
    {
        btn_ToggleMusic.onClick.AddListener(SoundManager.Instance.ToggleMusic);

        CloseAllPanels();

        AddScore();
    }

    #region  Elements

    public void AddScore()
    {
        PlayerData.Instance.Score++;
        PlayerPrefs.SetInt("Score", PlayerData.Instance.Score);
        txt_Score.text = PlayerData.Instance.Score.ToString();

        if (mySequence != null)
            mySequence.Kill();

        mySequence = DOTween.Sequence();
        txt_ScoreTransform.localScale = Vector3.one;
        mySequence.Append(txt_ScoreTransform.DOScale(1.5f, 0.2f).OnComplete(() => txt_Score.transform.DOScale(1f, 0.2f)));
    }
    #endregion

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
