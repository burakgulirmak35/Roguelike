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
    [SerializeField] private float defaultMusicVolume = 0.2f;
    [SerializeField] private float defaultSoundVolume = 1f;
    private float MusicVolume;

    private void StartMusic()
    {
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", defaultMusicVolume);
        SoundVolume = PlayerPrefs.GetFloat("SoundVolume", defaultSoundVolume);
        musicPlayer.clip = musicList;
        musicPlayer.loop = true;
        musicPlayer.volume = MusicVolume;
        musicPlayer.Play();
    }

    public void ChangeMusicVolume(float _value)
    {
        MusicVolume = _value;
        musicPlayer.volume = MusicVolume;
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
    }

    // --- SFX Pool ---

    [Header("SFX Pool")]
    [SerializeField] private AudioSource[] sfxPool;
    private int _poolIndex;

    [Header("SFX Gun Pool")]
    [SerializeField] private AudioSource[] sfxGunPool;
    private int _sfxGunPoolIndex;

    [Header("Sounds")]
    private float SoundVolume;

    public float GetMusicVolume() => MusicVolume;
    public float GetSoundVolume() => SoundVolume;

    public void ChangeSoundVolume(float _value)
    {
        SoundVolume = _value;
        PlayerPrefs.SetFloat("SoundVolume", SoundVolume);
    }
    [Header("---")]
    [SerializeField] private Sound GunSound;

    public void PlayGunSound()
    {
        PlayBulletDirect(GunSound);
    }

    public void PlaySound(Sound sound)
    {
        PlayDirect(sound);
    }

    private void PlayBulletDirect(Sound _audio)
    {
        if (_audio.audioClip == null || _audio.audioClip.Length == 0) return;

        AudioSource source = sfxGunPool[_sfxGunPoolIndex];
        _sfxGunPoolIndex = (_sfxGunPoolIndex + 1) % sfxGunPool.Length;

        source.Stop();
        source.clip = _audio.audioClip[Random.Range(0, _audio.audioClip.Length)];
        source.volume = _audio.volume * SoundVolume;
        source.pitch = Random.Range(_audio.minPitch, _audio.maxPitch);
        source.Play();
    }

    private void PlayDirect(Sound _audio)
    {
        if (_audio.audioClip == null || _audio.audioClip.Length == 0) return;

        AudioSource source = sfxPool[_poolIndex];
        _poolIndex = (_poolIndex + 1) % sfxPool.Length;

        source.Stop();
        source.clip = _audio.audioClip[Random.Range(0, _audio.audioClip.Length)];
        source.volume = _audio.volume * SoundVolume;
        source.pitch = Random.Range(_audio.minPitch, _audio.maxPitch);
        source.Play();
    }
}

[System.Serializable]
public class Sound
{
    public AudioClip[] audioClip;
    [Range(0, 2)] public float volume = 1f;
    [Range(0, 2)] public float minPitch = 1f;
    [Range(0, 2)] public float maxPitch = 1f;
}
