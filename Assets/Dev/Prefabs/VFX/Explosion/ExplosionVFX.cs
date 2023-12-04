using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionVFX : MonoBehaviour
{
    [SerializeField] private ExplosionSO explosionSO;
    private Collider[] hitColliders;
    private void Explode()
    {
        hitColliders = Physics.OverlapSphere(transform.position, explosionSO.Range);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].tag.Equals("Enemy"))
            {
                hitColliders[i].gameObject.GetComponent<IDamagable>().TakeDamage(explosionSO.Damage);
            }
        }
    }

    private void OnEnable()
    {
        Explode();
    }
}
