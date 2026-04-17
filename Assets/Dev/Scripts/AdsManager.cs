using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static AdsManager Instance { get; private set; }
    [SerializeField] private string _androidGameId = "6093566";
    [SerializeField] private bool _testMode = true;

    private const string INTERSTITIAL_ID = "Interstitial_Android";
    private const string REWARDED_ID = "Rewarded_Android";
    private const string BANNER_ID = "Banner_Android";

    private BannerLoadOptions _bannerLoadOptions;

    private bool _interstitialLoaded;
    private bool _rewardedLoaded;

    private Action _onInterstitialDone;
    private Action _onRewardedSuccess;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Advertisement.Initialize(_androidGameId, _testMode, this);
    }

    // ─── Initialization ────────────────────────────────────────────────────────

    public void OnInitializationComplete()
    {
        LoadInterstitial();
        LoadRewarded();
        _bannerLoadOptions = new BannerLoadOptions
        {
            loadCallback = () => { },
            errorCallback = (msg) => Debug.LogWarning($"[Ads] Banner load failed: {msg}")
        };
    }

    // ─── Banner ────────────────────────────────────────────────────────────────

    public void ShowBanner()
    {
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Load(BANNER_ID, _bannerLoadOptions);
        Advertisement.Banner.Show(BANNER_ID);
    }

    public void HideBanner()
    {
        Advertisement.Banner.Hide();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogWarning($"[Ads] Init failed: {error} – {message}");
    }

    // ─── Load ──────────────────────────────────────────────────────────────────

    private void LoadInterstitial() => Advertisement.Load(INTERSTITIAL_ID, this);
    private void LoadRewarded() => Advertisement.Load(REWARDED_ID, this);

    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (placementId == INTERSTITIAL_ID) _interstitialLoaded = true;
        if (placementId == REWARDED_ID) _rewardedLoaded = true;
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogWarning($"[Ads] Load failed: {placementId} – {error}");
    }

    // ─── Show ──────────────────────────────────────────────────────────────────

    /// <summary>
    /// Interstitial reklam göster. Bittikten (veya hata olunca) onDone çağrılır.
    /// Reklam hazır değilse onDone hemen çalışır.
    /// </summary>
    public void ShowInterstitial(Action onDone = null)
    {
        _onInterstitialDone = onDone;
        if (_interstitialLoaded)
        {
            _interstitialLoaded = false;
            Advertisement.Show(INTERSTITIAL_ID, this);
        }
        else
        {
            onDone?.Invoke();
        }
    }

    /// <summary>
    /// Rewarded reklam göster. Sadece reklam tamamlanırsa onSuccess çağrılır.
    /// Reklam hazır değilse onSuccess hemen çalışır (kullanıcıyı cezalandırma).
    /// </summary>
    public void ShowRewarded(Action onSuccess)
    {
        _onRewardedSuccess = onSuccess;
        if (_rewardedLoaded)
        {
            _rewardedLoaded = false;
            Advertisement.Show(REWARDED_ID, this);
        }
        else
        {
            onSuccess?.Invoke();
        }
    }

    // ─── Show callbacks ────────────────────────────────────────────────────────

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState state)
    {
        if (placementId == INTERSTITIAL_ID)
        {
            LoadInterstitial();
            _onInterstitialDone?.Invoke();
            _onInterstitialDone = null;
        }
        else if (placementId == REWARDED_ID)
        {
            LoadRewarded();
            if (state == UnityAdsShowCompletionState.COMPLETED)
            {
                _onRewardedSuccess?.Invoke();
                _onRewardedSuccess = null;
            }
        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogWarning($"[Ads] Show failed: {placementId} – {error}");
        if (placementId == INTERSTITIAL_ID) { _onInterstitialDone?.Invoke(); _onInterstitialDone = null; LoadInterstitial(); }
        if (placementId == REWARDED_ID) { _onRewardedSuccess?.Invoke(); _onRewardedSuccess = null; LoadRewarded(); }
    }

    public void OnUnityAdsShowStart(string placementId) { }
    public void OnUnityAdsShowClick(string placementId) { }
}
