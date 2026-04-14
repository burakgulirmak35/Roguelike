using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionVFX : MonoBehaviour
{
    [SerializeField] private ExplosionSO explosionSO;
    [SerializeField] private Sound sound;

    private int _enemyLayer;
    private readonly Collider[] hitColliders = new Collider[32];

    private void Awake()
    {
        _enemyLayer = LayerMask.GetMask("Enemy");
    }

    private void Explode()
    {
        SoundManager.Instance.PlaySound(sound);
        int count = Physics.OverlapSphereNonAlloc(transform.position, explosionSO.Range, hitColliders, _enemyLayer);
        for (int i = 0; i < count; i++)
        {
            hitColliders[i].GetComponent<Enemy>().enemyHealthSystem.TakeDamage(explosionSO.Damage);
        }
    }

    private void OnEnable()
    {
        Explode();
    }
}
