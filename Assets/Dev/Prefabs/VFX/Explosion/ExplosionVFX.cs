using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionVFX : MonoBehaviour
{
    [SerializeField] private ExplosionSO explosionSO;
    [SerializeField] private Sound sound;

    private Collider[] hitColliders;
    private void Explode()
    {
        SoundManager.Instance.PlaySound(sound);
        hitColliders = Physics.OverlapSphere(transform.position, explosionSO.Range);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].tag.Equals("Enemy"))
            {
                hitColliders[i].gameObject.GetComponent<Enemy>().enemyHealthSystem.TakeDamage(explosionSO.Damage);
            }
        }
    }

    private void OnEnable()
    {
        Explode();
    }
}
