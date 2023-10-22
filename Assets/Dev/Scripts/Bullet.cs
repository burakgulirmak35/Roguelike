using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] BulletParticles;
    [HideInInspector] public float Damage;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
