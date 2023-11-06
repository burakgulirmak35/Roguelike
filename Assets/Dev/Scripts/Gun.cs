using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private PoolManager poolManager;
    private SoundManager soundManager;
    private PlayerData playerData;
    [Header("Parts")]
    [SerializeField] private Transform[] firePoint;
    [SerializeField] private Transform LeftHandPos;
    [Header("VFX")]
    [SerializeField] private ParticleSystem Muzzle;

    [Space]
    private Coroutine FireCoro;
    private bool isFire;

    private void Awake()
    {
        poolManager = FindObjectOfType<PoolManager>();
        soundManager = FindObjectOfType<SoundManager>();
        playerData = FindObjectOfType<PlayerData>();
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
    private IEnumerator FireLoop()
    {
        while (isFire)
        {
            yield return new WaitForSeconds(1.0f / playerData.FireRate);
            for (int i = 0; i < firePoint.Length; i++)
            {
                for (int j = 0; j < playerData.BurstCount; j++)
                {
                    if (isFire)
                    {
                        tmpBullet = poolManager.GetFromPool(PoolTypes.Bullet);
                        tmpBullet.transform.position = firePoint[i].position;
                        tmpBullet.transform.forward = firePoint[i].forward;

                        Muzzle.Play();
                        tmpBullet.SetActive(true);
                        soundManager.PlayGunSound(playerData.SelectedGun);
                        tmpBullet.GetComponent<Rigidbody>().AddForce(tmpBullet.transform.forward * playerData.BulletSpeed, ForceMode.Impulse);

                        yield return new WaitForSeconds(playerData.EachBurstTime);
                    }
                }
            }
        }
    }

}
