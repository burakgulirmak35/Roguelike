using System;
using UnityEngine;
using UnityEngine.UI;

public class PanelPrivacy : MonoBehaviour
{
    [SerializeField] private Button btn_Ok;
    [SerializeField] private Button btn_PrivacyPolicy;

    private const string PREFS_KEY = "PrivacyAccepted";
    private const string PRIVACY_URL = "https://your-privacy-policy-url.com"; // Privacy policy linkini buraya yaz

    public event Action OnAccepted;

    void Awake()
    {
        btn_Ok.onClick.AddListener(Accept);
        btn_PrivacyPolicy.onClick.AddListener(() => Application.OpenURL(PRIVACY_URL));
    }

    void OnDestroy()
    {
        btn_Ok.onClick.RemoveListener(Accept);
    }

    public bool IsAccepted() => PlayerPrefs.GetInt(PREFS_KEY, 0) == 1;

    private void Accept()
    {
        PlayerPrefs.SetInt(PREFS_KEY, 1);
        PlayerPrefs.Save();
        gameObject.SetActive(false);
        OnAccepted?.Invoke();
    }
}
