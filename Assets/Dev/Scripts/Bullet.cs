using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] BulletParticles;
    [SerializeField] private Rigidbody rb;
    [HideInInspector] public float Damage;
    private PoolManager poolManager;
    private GameObject tempObject;

    private void Awake()
    {
        poolManager = FindObjectOfType<PoolManager>();
    }

    private void OnEnable()
    {
        for (int i = 0; i < BulletParticles.Length; i++)
        {
            BulletParticles[i].Stop();
            BulletParticles[i].time = 0;
            BulletParticles[i].Play();
        }
        DisableCoro = StartCoroutine(DisableTimer());
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Enemy":
                other.gameObject.GetComponent<IDamagable>().TakeDamage(Damage);
                tempObject = poolManager.GetFromPool(PoolTypes.BloodShot);
                tempObject.transform.position = transform.position;
                tempObject.SetActive(true);
                Disable();
                break;
            default:
                break;
        }
    }

    private Coroutine DisableCoro;
    private IEnumerator DisableTimer()
    {
        yield return new WaitForSeconds(2f);
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
