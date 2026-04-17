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
    [Header("AutoAim")]
    [SerializeField] private GameObject img_ToggleAim;
    [SerializeField] private GameObject img_AutoAim;
    [SerializeField] private GameObject img_ManuelAim;
    [Header("HoverBoard")]
    [SerializeField] public Button btn_HoverBoard;
    [SerializeField] public Image img_HoverBoard;
    [Header("Settings")]
    [SerializeField] private Button btn_Settings;

    [Header("Panels")]
    [SerializeField] public PanelPlayerDead panelPlayerDead;
    [SerializeField] public PanelUpgrade panelUpgrade;
    [SerializeField] public PanelFadeInOut panelFadeInOut;
    [SerializeField] public PanelSettings panelSettings;
    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI txt_Score;
    private Transform txt_ScoreTransform;

    [Header("Damage Effect")]
    [SerializeField] private Image img_DamageEffect;
    [SerializeField] private float damageEffectDuration = 0.4f;
    [SerializeField] private float damageEffectIncrement = 0.2f;
    private Tweener _damageTween;

    [Header("DG.T")]
    private Sequence mySequence;


    private void Awake()
    {
        Instance = this;
        panelFadeInOut.FadeIn(1f);
        LeftJoystickBasePos = LeftJoystick.position;
        txt_ScoreTransform = txt_Score.transform;
        img_DamageEffect.color = new Color(1, 1, 1, 0);
    }

    private void OnEnable()
    {
        GameEvents.OnEnemyKilled += OnEnemyKilled;
        GameEvents.OnPlayerDead += OnPlayerDead;
        GameEvents.OnLevelUp += OnLevelUp;
        GameEvents.OnDamageTaken += OnDamageTaken;
    }

    private void OnDisable()
    {
        GameEvents.OnEnemyKilled -= OnEnemyKilled;
        GameEvents.OnPlayerDead -= OnPlayerDead;
        GameEvents.OnLevelUp -= OnLevelUp;
        GameEvents.OnDamageTaken -= OnDamageTaken;
    }

    private void Start()
    {
        btn_Settings.onClick.AddListener(() => EnablePanelSettings(true));
        CloseAllPanels();
        txt_Score.text = PlayerData.Instance.Score.ToString();
    }

    #region Score

    private void OnEnemyKilled(Transform t, Vector3 pos)
    {
        PlayerData.Instance.Score++;
        txt_Score.text = PlayerData.Instance.Score.ToString();

        if (mySequence != null) mySequence.Kill();
        mySequence = DOTween.Sequence();
        txt_ScoreTransform.localScale = Vector3.one;
        mySequence.Append(txt_ScoreTransform.DOScale(1.5f, 0.2f)
            .OnComplete(() => txt_Score.transform.DOScale(1f, 0.2f)));
    }

    #endregion


    private void CloseAllPanels()
    {
        EnablePanelUpgrade(false);
        EnablePanelPlayerDead(false);
        EnablePanelSettings(false);
    }

    public void EnablePanelSettings(bool _state)
    {
        panelSettings.gameObject.SetActive(_state);
        if (_state) GameManager.Instance.FreezeGame();
        else GameManager.Instance.ResumeGame();
    }

    private void OnPlayerDead()
    {
        CameraManager.Instance.CamDeathPos();
        panelPlayerDead.gameObject.SetActive(true);
        AdsManager.Instance.ShowBanner();
    }

    public void EnablePanelPlayerDead(bool _state)
    {
        if (_state)
        {
            CameraManager.Instance.CamDeathPos();
            AdsManager.Instance.ShowBanner();
        }
        else
        {
            AdsManager.Instance.HideBanner();
        }
        panelPlayerDead.gameObject.SetActive(_state);
    }

    private void OnDamageTaken(float amount, Vector3 pos, bool isPlayer)
    {
        if (!isPlayer) return;
        _damageTween?.Kill();
        float newAlpha = Mathf.Min(img_DamageEffect.color.a + damageEffectIncrement, 1f);
        img_DamageEffect.color = new Color(1, 1, 1, newAlpha);
        _damageTween = img_DamageEffect.DOFade(0f, damageEffectDuration);
    }

    private void OnLevelUp(int level)
    {
        EnablePanelUpgrade(true);
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
