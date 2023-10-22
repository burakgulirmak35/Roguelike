using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public float volume;
    [Header("----------------------------")]
    public Sound GrenadeExplosion;
    public Sound RocketExplosion;
    [Header("----------------------------")]
    public Sound Pistol;
    public Sound Rifle;
    public Sound Shotgun;
    public Sound Sniper;
    public Sound Grenade;
    public Sound Minigun;
    public Sound Rocket;

    private void Awake()
    {
        Instance = this;
    }

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
        audioSource.volume = Random.Range(_audio.minVolume, _audio.maxVolume);
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
