using UnityEngine;
using UnityEngine.UI;

public class PanelSettings : MonoBehaviour
{
    [SerializeField] private Button btn_Close;
    [SerializeField] private Slider slider_Music;
    [SerializeField] private Slider slider_Effect;

    void Awake()
    {
        btn_Close.onClick.AddListener(Close);
        slider_Music.onValueChanged.AddListener(SoundManager.Instance.ChangeMusicVolume);
        slider_Effect.onValueChanged.AddListener(SoundManager.Instance.ChangeSoundVolume);
    }

    private void OnEnable()
    {
        slider_Music.value = SoundManager.Instance.GetMusicVolume();
        slider_Effect.value = SoundManager.Instance.GetSoundVolume();
    }

    private void OnDestroy()
    {
        btn_Close.onClick.RemoveListener(Close);
        slider_Music.onValueChanged.RemoveListener(SoundManager.Instance.ChangeMusicVolume);
        slider_Effect.onValueChanged.RemoveListener(SoundManager.Instance.ChangeSoundVolume);
    }

    public void Close()
    {
        UIManager.Instance.EnablePanelSettings(false);
    }
}
