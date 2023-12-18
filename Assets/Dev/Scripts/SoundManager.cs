using System.Collections;
using System.Collections.Generic;
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

    [Header("Music----------------------------")]
    [SerializeField] private AudioClip musicList;
    private AudioSource musicPlayer;
    private float MusicVolume;

    private void StartMusic()
    {
        musicPlayer = gameObject.AddComponent<AudioSource>();
        musicPlayer.volume = MusicVolume;
        musicPlayer.clip = musicList;
        musicPlayer.dopplerLevel = 0;
        musicPlayer.reverbZoneMix = 0;
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
            ChangeMusicVolume(0.5f);

            UIManager.Instance.img_MusicOn.SetActive(true);
            UIManager.Instance.img_MusicOff.SetActive(false);
        }
    }

    public void CheckMusic()
    {
        if (PlayerPrefs.GetInt("isMusic", 1) == 1)
        {
            ChangeMusicVolume(0.5f);

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


    [Header("Sounds----------------------------")]
    [SerializeField] private float SoundVolume;
    [Header("----------------------------")]
    [SerializeField] private Sound GrenadeExplosion;
    [SerializeField] private Sound RocketExplosion;
    [Header("----------------------------")]
    [SerializeField] private Sound Pistol;
    [SerializeField] private Sound Rifle;
    [SerializeField] private Sound Shotgun;
    [SerializeField] private Sound Sniper;
    [SerializeField] private Sound Grenade;
    [SerializeField] private Sound Minigun;
    [SerializeField] private Sound Rocket;

    public void PlayGunSound(GunType gunType)
    {
        switch (gunType)
        {
            case GunType.Pistol:
                PlayDirect(Pistol);
                break;
            case GunType.Rifle:
                PlayDirect(Rifle);
                break;
            case GunType.ShotGun:
                PlayDirect(Shotgun);
                break;
            case GunType.Sniper:
                PlayDirect(Sniper);
                break;
            case GunType.Grenade:
                PlayDirect(Grenade);
                break;
            case GunType.Minigun:
                PlayDirect(Minigun);
                break;
            case GunType.Rocket:
                PlayDirect(Rocket);
                break;
        }
    }

    private void PlayDirect(Sound _audio)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.playOnAwake = false;
        audioSource.clip = _audio.audioClip[Random.Range(0, _audio.audioClip.Length)];
        audioSource.dopplerLevel = 0;
        audioSource.reverbZoneMix = 0;
        audioSource.volume = Random.Range(_audio.minVolume, _audio.maxVolume) * SoundVolume;
        audioSource.pitch = Random.Range(_audio.minPitch, _audio.maxPitch);
        audioSource.Play();
        Destroy(audioSource, 1f);
    }

    [System.Serializable]
    public class Sound
    {
        public AudioClip[] audioClip;
        [Range(0, 2)]
        public float minVolume = 1;
        [Range(0, 2)]
        public float maxVolume = 1;
        [Range(0, 2)]
        public float minPitch = 1;
        [Range(0, 2)]
        public float maxPitch = 1;
    }
}
