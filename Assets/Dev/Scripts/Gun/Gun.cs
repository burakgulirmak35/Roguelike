using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private PoolManager poolManager;
    private SoundManager soundManager;
    private PlayerData playerData;
    private CameraManager cameraManager;
    [Header("Parts")]
    [SerializeField] public Transform[] firePoint;
    [SerializeField] private Transform LeftHandPos;
    [Header("VFX")]
    [SerializeField] private ParticleSystem Muzzle;

    [Space]
    private Coroutine FireCoro;
    private bool isFire;
    private WaitForSeconds _waitBurst;

    private void Awake()
    {
        poolManager = FindFirstObjectByType<PoolManager>();
        soundManager = FindFirstObjectByType<SoundManager>();
        playerData = FindFirstObjectByType<PlayerData>();
        cameraManager = FindFirstObjectByType<CameraManager>();
    }

    private void Start()
    {
        _waitBurst = new WaitForSeconds(playerData.EachBurstTime);
    }

    public Transform GetLeftHandPos()
    {
        return LeftHandPos;
    }

    public void StartFire()
    {
        if (!isFire)
        {
            if (FireCoro != null)
            {
                StopCoroutine(FireCoro);
                FireCoro = null;
            }
            isFire = true;
            FireCoro = StartCoroutine(FireLoop());
        }
    }

    public void StopFire()
    {
        if (FireCoro != null)
        {
            StopCoroutine(FireCoro);
            FireCoro = null;
        }
        isFire = false;
    }

    private GameObject tmpBullet;
    private Transform tmpBulletTransform;
    private Rigidbody tmpBulletRB;
    private IEnumerator FireLoop()
    {
        float elapsed = 0f;
        while (isFire)
        {
            elapsed += Time.deltaTime;
            float interval = 1.0f / (playerData.FireRate * playerData.FireRateMultipler);
            if (elapsed >= interval)
            {
                elapsed -= interval;
                for (int i = 0; i < firePoint.Length; i++)
                {
                    for (int j = 0; j < playerData.BurstCount; j++)
                    {
                        if (!isFire) yield break;
                        cameraManager.ShakeCamera();

                        tmpBullet = poolManager.GetFromPool(PoolTypes.Bullet);
                        tmpBulletTransform = tmpBullet.transform;
                        tmpBulletRB = tmpBullet.GetComponent<Rigidbody>();

                        tmpBulletTransform.position = firePoint[i].position;
                        tmpBulletTransform.forward = firePoint[i].forward;

                        Muzzle.Play();
                        soundManager.PlayGunSound(playerData.SelectedGun);

                        tmpBullet.SetActive(true);
                        tmpBulletRB.AddForce(tmpBulletTransform.forward * playerData.BulletSpeed, ForceMode.Impulse);

                        yield return _waitBurst;
                    }
                }
            }
            yield return null;
        }
    }

}
