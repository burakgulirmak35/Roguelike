using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private CollectableSO collectableSO;

    private SphereCollider myCollider;
    private Transform myTransform;

    void Awake()
    {
        myCollider = GetComponent<SphereCollider>();
        myTransform = this.transform;
    }

    private void OnEnable()
    {
        myCollider.enabled = true;
    }

    public void Collect()
    {
        myTransform.parent = Player.Instance.PlayerTransform;
        myTransform.DOLocalJump(collectableSO.LocalCollectedPos, 2, 1, 0.2f).OnComplete(() => OnCollected());
        myCollider.enabled = false;
    }

    private void OnCollected()
    {
        switch (collectableSO.itemType)
        {
            case ItemType.Experience:
                Experience();
                break;
            case ItemType.Bomb:
                Bomb();
                break;
            case ItemType.FireRateBoost:
                FireRateBoost();
                break;
            case ItemType.Health:
                Health();
                break;
            case ItemType.MeshTrain:
                MeshTrain();
                break;
            case ItemType.Magnet:
                Magnet();
                break;
            case ItemType.SlowMotion:
                SlowMotion();
                break;
            case ItemType.SpeedBoost:
                SpeedBoost();
                break;
        }
    }

    private void Experience()
    {
        Player.Instance.levelSystem.AddExperience(1);

        myTransform.parent = PoolManager.Instance.ExperienceHolder;
        gameObject.SetActive(false);
    }

    private void Bomb()
    {
        Spawner.Instance.SpawnAtPos(PoolTypes.SimpleExplosion, myTransform.position);

        myTransform.parent = PoolManager.Instance.CollectableHolder;
        gameObject.SetActive(false);
    }

    private void FireRateBoost()
    {
        Player.Instance.effectSystem.BoostFireRate();

        myTransform.parent = PoolManager.Instance.CollectableHolder;
        gameObject.SetActive(false);
    }

    private void Health()
    {
        Player.Instance.healthSystem.HealPercent(0.25f);

        myTransform.parent = PoolManager.Instance.CollectableHolder;
        gameObject.SetActive(false);
    }

    private void MeshTrain()
    {
        Player.Instance.effectSystem.meshTrail.ActiveTrail();

        myTransform.parent = PoolManager.Instance.CollectableHolder;
        gameObject.SetActive(false);
    }

    private void Magnet()
    {
        Spawner.Instance.SpawnAtPos(PoolTypes.Magnet, Player.Instance.effectSystem.EffectTransform.position);

        myTransform.parent = PoolManager.Instance.CollectableHolder;
        gameObject.SetActive(false);
    }

    private void SlowMotion()
    {
        GameManager.Instance.SlowMotion();

        myTransform.parent = PoolManager.Instance.CollectableHolder;
        gameObject.SetActive(false);
    }

    private void SpeedBoost()
    {
        Player.Instance.effectSystem.BoostMovementSpeed();

        myTransform.parent = PoolManager.Instance.CollectableHolder;
        gameObject.SetActive(false);
    }
}
