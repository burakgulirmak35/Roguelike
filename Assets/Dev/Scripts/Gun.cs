using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private PoolManager poolManager;
    private SoundManager soundManager;
    [Header("Stats")]
    [SerializeField] private GunSO gunSO;
    [Header("Parts")]
    [SerializeField] private Transform[] firePoint;
    [SerializeField] private Transform LeftHandPos;

    [Space]
    private Coroutine FireCoro;
    private bool isFire;

    private void Start()
    {
        poolManager = FindObjectOfType<PoolManager>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    public GunSO GetGunSO()
    {
        return gunSO;
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

    private IEnumerator FireLoop()
    {
        while (isFire)
        {
            yield return new WaitForSeconds(1.0f / gunSO.FireRate);
            for (int i = 0; i < firePoint.Length; i++)
            {
                for (int j = 0; j < gunSO.BurstCount; j++)
                {
                    if (isFire)
                    {
                        GameObject tmpBullet = poolManager.GetFromPool(PoolTypes.Bullet);
                        Bullet tmpBulletSC = tmpBullet.GetComponent<Bullet>();
                        tmpBullet.transform.position = firePoint[i].position;
                        tmpBullet.transform.forward = firePoint[i].forward;
                        tmpBulletSC.Damage = gunSO.Damage;

                        tmpBullet.SetActive(true);
                        soundManager.PlayGunSound(gunSO.gunType);
                        tmpBullet.GetComponent<Rigidbody>().AddForce(tmpBullet.transform.forward * gunSO.BulletSpeed, ForceMode.Impulse);

                        yield return new WaitForSeconds(gunSO.EachBurstTime);
                    }
                }
            }
        }
    }

}
