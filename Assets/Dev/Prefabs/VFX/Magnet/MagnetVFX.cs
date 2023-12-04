using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetVFX : MonoBehaviour
{
    [SerializeField] private MagnetSO magnetSO;

    private Collider[] hitColliders;
    private IEnumerator Pull()
    {
        for (int repeat = 0; repeat < magnetSO.RepeatTime; repeat++)
        {
            yield return new WaitForSeconds(0.2f);
            hitColliders = Physics.OverlapSphere(transform.position, magnetSO.Range);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].tag.Equals("Collectable"))
                {
                    hitColliders[i].GetComponent<Collectable>().Collect();

                }
            }
        }
    }

    private void OnEnable()
    {
        StartCoroutine(Pull());
    }
}
