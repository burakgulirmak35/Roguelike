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
            case ItemType.Booster:
                Booster();
                break;
            case ItemType.Health:
                Health();
                break;
            case ItemType.HowerBoard:
                HowerBoard();
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
        myTransform.parent = PoolManager.Instance.ExperienceHolder;
        gameObject.SetActive(false);

        Player.Instance.AddExperience(1);
    }

    private void Bomb()
    {
        myTransform.parent = PoolManager.Instance.CollectableHolder;
        gameObject.SetActive(false);

        Spawner.Instance.SpawnAtPos(PoolTypes.SimpleExplosion, myTransform.position);
    }

    private void Booster()
    {
        myTransform.parent = PoolManager.Instance.CollectableHolder;
        gameObject.SetActive(false);
    }

    private void Health()
    {
        myTransform.parent = PoolManager.Instance.CollectableHolder;
        gameObject.SetActive(false);

        Player.Instance.healthSystem.HealPercent(0.25f);
    }

    private void HowerBoard()
    {
        myTransform.parent = PoolManager.Instance.CollectableHolder;
        gameObject.SetActive(false);
    }

    private void Magnet()
    {
        myTransform.parent = PoolManager.Instance.CollectableHolder;
        gameObject.SetActive(false);
    }

    private void SlowMotion()
    {
        myTransform.parent = PoolManager.Instance.CollectableHolder;
        gameObject.SetActive(false);

        GameManager.Instance.SlowMotion();
    }

    private void SpeedBoost()
    {
        myTransform.parent = PoolManager.Instance.CollectableHolder;
        gameObject.SetActive(false);
    }
}
