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

    private void Start()
    {
        poolManager = PoolManager.Instance;
        soundManager = SoundManager.Instance;
        playerData = PlayerData.Instance;
        cameraManager = CameraManager.Instance;
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
    private Bullet tmpBulletComp;
    private Transform tmpBulletTransform;
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
                        tmpBulletComp = tmpBullet.GetComponent<Bullet>();
                        tmpBulletTransform = tmpBullet.transform;

                        tmpBulletTransform.position = firePoint[i].position;
                        tmpBulletTransform.forward = firePoint[i].forward;

                        Muzzle.Play();
                        soundManager.PlayGunSound();

                        tmpBullet.SetActive(true);
                        tmpBulletComp.Rb.AddForce(tmpBulletTransform.forward * playerData.BulletSpeed, ForceMode.Impulse);

                        yield return _waitBurst;
                    }
                }
            }
            yield return null;
        }
    }

}
