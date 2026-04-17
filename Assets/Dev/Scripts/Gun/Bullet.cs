using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] BulletParticles;
    [SerializeField] private Rigidbody rb;
    public Rigidbody Rb => rb;
    private Transform bulletTransform;
    private int bounceCount;
    private GameObject tempObject;

    private static readonly WaitForSeconds _waitDisable = new WaitForSeconds(3f);

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
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<Enemy>(out var enemy))
                enemy.enemyHealthSystem.TakeDamage(PlayerData.Instance.Damage);

            tempObject = PoolManager.Instance.GetFromPool(PoolTypes.BloodShot);
            tempObject.transform.position = transform.position;
            tempObject.SetActive(true);

            if (PlayerData.Instance.ExplosiveAmmo) { Spawner.Instance.SpawnAtPos(PoolTypes.BulletExplosion, transform.position); }
            if (PlayerData.Instance.Penetrability) { Penetration(); }
            else { Disable(); }
        }
        else if (other.CompareTag("Enviroment"))
        {
            if (bounceCount > 0) { Bounce(); }
            else { Disable(); }
        }
        else if (other.CompareTag("Props"))
        {
            if (Player.Instance.playerState.Equals(PlayerState.HoverBoard)) return;
            if (bounceCount > 0) { Bounce(); }
            else { Disable(); }
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

        if (Physics.Raycast(transform.position - Vector3.forward * 1, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, LayerMask.GetMask("Wall")))
        {
            dir = Vector3.Reflect(rb.linearVelocity.normalized, hit.normal);
            rb.linearVelocity = Vector3.zero;
            transform.forward = dir;
            rb.linearVelocity = transform.forward * PlayerData.Instance.BulletSpeed;
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
        yield return _waitDisable;
        Disable();
    }

    private void Disable()
    {
        if (DisableCoro != null)
        {
            StopCoroutine(DisableCoro);
            DisableCoro = null;
        }

        rb.linearVelocity = Vector3.zero;
        gameObject.SetActive(false);
    }


}
