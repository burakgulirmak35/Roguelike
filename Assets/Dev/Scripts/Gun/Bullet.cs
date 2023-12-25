using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] BulletParticles;
    [SerializeField] private Rigidbody rb;
    private Transform bulletTransform;
    private int bounceCount;
    private GameObject tempObject;

    void Awake()
    {
        bulletTransform = transform;
    }

    private void OnEnable()
    {
        for (int i = 0; i < BulletParticles.Length; i++)
        {
            BulletParticles[i].Stop();
            BulletParticles[i].time = 0;
            BulletParticles[i].Play();
        }
        bounceCount = PlayerData.Instance.BounceCount;
        StartDisableTimer();
    }


    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Enemy":
                other.gameObject.GetComponent<Enemy>().enemyHealthSystem.TakeDamage(PlayerData.Instance.Damage);
                tempObject = PoolManager.Instance.GetFromPool(PoolTypes.BloodShot);
                tempObject.transform.position = transform.position;
                tempObject.SetActive(true);

                if (PlayerData.Instance.ExplosiveAmmo) { Spawner.Instance.SpawnAtPos(PoolTypes.BulletExplosion, transform.position); }
                if (PlayerData.Instance.Penetrability) { Penetration(); }
                else { Disable(); }

                break;
            case "Enviroment":

                if (bounceCount > 0) { Bounce(); }
                else { Disable(); }

                break;
            default:
                break;
        }
    }

    private void Penetration()
    {
        StartDisableTimer();
    }

    private RaycastHit hit;
    private Vector3 dir;
    private void Bounce()
    {
        bounceCount--;
        StartDisableTimer();

        if (Physics.Raycast(transform.position - Vector3.forward * 1, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, 7))
        {
            dir = Vector3.Reflect(rb.velocity.normalized, hit.normal);
            rb.velocity = Vector3.zero;
            transform.forward = dir;
            rb.velocity = transform.forward * PlayerData.Instance.BulletSpeed;
        }
    }

    private Coroutine DisableCoro;
    private void StartDisableTimer()
    {
        if (DisableCoro != null)
            StopCoroutine(DisableCoro);

        DisableCoro = StartCoroutine(DisableTimer());
    }

    private IEnumerator DisableTimer()
    {
        yield return new WaitForSeconds(3f);
        Disable();
    }

    private void Disable()
    {
        if (DisableCoro != null)
        {
            StopCoroutine(DisableCoro);
            DisableCoro = null;
        }

        rb.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }


}
