using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetVFX : MonoBehaviour
{
    [SerializeField] private MagnetSO magnetSO;
    [SerializeField] private Sound sound;

    private int _collectableLayer;
    private static readonly WaitForSeconds _waitPull = new WaitForSeconds(0.4f);
    private readonly Collider[] hitColliders = new Collider[64];

    private void Awake()
    {
        _collectableLayer = LayerMask.GetMask("Collectable");
    }

    private IEnumerator Pull()
    {
        for (int repeat = 0; repeat < magnetSO.RepeatTime; repeat++)
        {
            SoundManager.Instance.PlaySound(sound);
            yield return _waitPull;
            int count = Physics.OverlapSphereNonAlloc(transform.position, magnetSO.Range, hitColliders, _collectableLayer);
            for (int i = 0; i < count; i++)
            {
                hitColliders[i].GetComponent<Collectable>().Collect();
            }
        }
    }

    private void OnEnable()
    {
        StartCoroutine(Pull());
    }
}
