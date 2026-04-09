using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartMusic();
    }

    // --- Music ---

    [Header("Music")]
    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioClip musicList;
    [SerializeField] private float defaultMusicVolume = 0.5f;
    private float MusicVolume;

    private void StartMusic()
    {
        musicPlayer.volume = MusicVolume;
        musicPlayer.clip = musicList;
        musicPlayer.loop = true;
        musicPlayer.Play();
        CheckMusic();
    }

    public void ChangeMusicVolume(float _value)
    {
        MusicVolume = _value;
        musicPlayer.volume = MusicVolume;
    }

    public void ToggleMusic()
    {
        if (PlayerPrefs.GetInt("isMusic", 1) == 1)
        {
            PlayerPrefs.SetInt("isMusic", 0);
            ChangeMusicVolume(0);
            UIManager.Instance.img_MusicOn.SetActive(false);
            UIManager.Instance.img_MusicOff.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetInt("isMusic", 1);
            ChangeMusicVolume(defaultMusicVolume);
            UIManager.Instance.img_MusicOn.SetActive(true);
            UIManager.Instance.img_MusicOff.SetActive(false);
        }
    }

    public void CheckMusic()
    {
        if (PlayerPrefs.GetInt("isMusic", 1) == 1)
        {
            ChangeMusicVolume(defaultMusicVolume);
            UIManager.Instance.img_MusicOn.SetActive(true);
            UIManager.Instance.img_MusicOff.SetActive(false);
        }
        else
        {
            ChangeMusicVolume(0);
            UIManager.Instance.img_MusicOn.SetActive(false);
            UIManager.Instance.img_MusicOff.SetActive(true);
        }
    }

    // --- SFX Pool ---

    [Header("SFX Pool")]
    [SerializeField] private AudioSource[] sfxPool;
    private int _poolIndex;

    [Header("Sounds")]
    [SerializeField] private float SoundVolume = 1f;
    [Header("---")]
    [SerializeField] private Sound GrenadeExplosion;
    [SerializeField] private Sound RocketExplosion;
    [Header("---")]
    [SerializeField] private Sound Pistol;
    [SerializeField] private Sound Rifle;
    [SerializeField] private Sound Shotgun;
    [SerializeField] private Sound Sniper;
    [SerializeField] private Sound Grenade;
    [SerializeField] private Sound Minigun;
    [SerializeField] private Sound Rocket;

    public void PlayGunSound(GunType gunType)
    {
        Sound sound = gunType switch
        {
            GunType.Pistol   => Pistol,
            GunType.Rifle    => Rifle,
            GunType.ShotGun  => Shotgun,
            GunType.Sniper   => Sniper,
            GunType.Grenade  => Grenade,
            GunType.Minigun  => Minigun,
            GunType.Rocket   => Rocket,
            _                => Pistol,
        };
        PlayDirect(sound);
    }

    public void PlaySound(Sound sound)
    {
        PlayDirect(sound);
    }

    private void PlayDirect(Sound _audio)
    {
        if (_audio.audioClip == null || _audio.audioClip.Length == 0) return;

        AudioSource source = sfxPool[_poolIndex];
        _poolIndex = (_poolIndex + 1) % sfxPool.Length;

        source.Stop();
        source.clip = _audio.audioClip[Random.Range(0, _audio.audioClip.Length)];
        source.volume = Random.Range(_audio.minVolume, _audio.maxVolume) * SoundVolume;
        source.pitch = Random.Range(_audio.minPitch, _audio.maxPitch);
        source.Play();
    }
}

[System.Serializable]
public class Sound
{
    public AudioClip[] audioClip;
    [Range(0, 2)] public float minVolume = 1f;
    [Range(0, 2)] public float maxVolume = 1f;
    [Range(0, 2)] public float minPitch = 1f;
    [Range(0, 2)] public float maxPitch = 1f;
}
