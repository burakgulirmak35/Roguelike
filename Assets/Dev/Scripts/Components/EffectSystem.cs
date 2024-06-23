using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSystem : MonoBehaviour
{
    [Header("MeshTrail")]
    [SerializeField] public MeshTrail meshTrail;
    [Header("FireRatePowerEffect")]
    [SerializeField] public GameObject FireRatePowerEffect;
    [SerializeField] private Sound FireRatePowerEffectSound;
    [Header("WindTrail")]
    [SerializeField] public GameObject WindTrail;
    [SerializeField] private Sound WindTrailSound;
    [Space]
    [SerializeField] public Transform EffectTransform;


    public void SetEffects()
    {
        meshTrail.skinnedMeshRenderer = Player.Instance.costumeSystem.GetSkinnedMesh();
    }

    #region FireRateBoost
    private bool isFireRateBoost;
    public void BoostFireRate()
    {
        if (!isFireRateBoost)
        {
            isFireRateBoost = true;
            StartCoroutine(BoostFireRateTimer());
        }
    }
    private IEnumerator BoostFireRateTimer()
    {
        SoundManager.Instance.PlaySound(FireRatePowerEffectSound);

        PlayerData.Instance.FireRateMultipler = 4f;
        Player.Instance.effectSystem.FireRatePowerEffect.SetActive(true);
        yield return new WaitForSeconds(3f);
        Player.Instance.effectSystem.FireRatePowerEffect.SetActive(false);
        PlayerData.Instance.FireRateMultipler = 1f;
        isFireRateBoost = false;
    }
    #endregion

    #region MovementSpeedBoost
    private bool isMovementSpeedBoost;
    public void BoostMovementSpeed()
    {
        if (!isMovementSpeedBoost)
        {
            isMovementSpeedBoost = true;
            StartCoroutine(BoostMovementSpeedTimer());
        }
    }
    private IEnumerator BoostMovementSpeedTimer()
    {
        SoundManager.Instance.PlaySound(WindTrailSound);

        PlayerData.Instance.MovementSpeedMultipler = 2f;
        Player.Instance.effectSystem.WindTrail.SetActive(true);
        yield return new WaitForSeconds(3f);
        Player.Instance.effectSystem.WindTrail.SetActive(false);
        PlayerData.Instance.MovementSpeedMultipler = 1f;
        isMovementSpeedBoost = false;
    }
    #endregion
}
