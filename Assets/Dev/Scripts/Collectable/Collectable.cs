using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;



public class Collectable : MonoBehaviour
{
    [SerializeField] private CollectableSO collectableSO;
    private SphereCollider myCollider;

    void Awake()
    {
        myCollider = GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {
        myCollider.enabled = true;
    }

    public void Collect()
    {
        transform.parent = Player.Instance.PlayerTransform;
        transform.DOLocalJump(collectableSO.LocalCollectedPos, 2, 1, 0.2f).OnComplete(() => OnCollected());
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
                SLowMotion();
                break;
            case ItemType.SpeedBoost:
                SpeedBoost();
                break;
        }
    }

    private void Experience()
    {
        transform.parent = PoolManager.Instance.ExperienceHolder;
        Player.Instance.AddExperience(1);
        gameObject.SetActive(false);
    }

    private void Bomb()
    {
        transform.parent = PoolManager.Instance.CollectableHolder;
        gameObject.SetActive(false);
    }

    private void Booster()
    {
        transform.parent = PoolManager.Instance.CollectableHolder;
        gameObject.SetActive(false);
    }

    private void Health()
    {
        transform.parent = PoolManager.Instance.CollectableHolder;
        gameObject.SetActive(false);
    }

    private void HowerBoard()
    {
        transform.parent = PoolManager.Instance.CollectableHolder;
        gameObject.SetActive(false);
    }

    private void Magnet()
    {
        transform.parent = PoolManager.Instance.CollectableHolder;
        gameObject.SetActive(false);
    }

    private void SLowMotion()
    {
        transform.parent = PoolManager.Instance.CollectableHolder;
        gameObject.SetActive(false);
    }

    private void SpeedBoost()
    {
        transform.parent = PoolManager.Instance.CollectableHolder;
        gameObject.SetActive(false);
    }
}
