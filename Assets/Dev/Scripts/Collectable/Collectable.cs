using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum ItemType
{
    Experience,
}

public class Collectable : MonoBehaviour
{
    [SerializeField] private ItemType itemType;
    private SphereCollider myCollider;
    private Vector3 LocalCollectedPos = new Vector3(0, 1, 0);

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
        transform.DOLocalJump(LocalCollectedPos, 2, 1, 0.2f).OnComplete(() => OnCollected());
        myCollider.enabled = false;
    }


    private void OnCollected()
    {
        transform.parent = PoolManager.Instance.ExperienceHolder;
        Player.Instance.AddExperience(1);
        gameObject.SetActive(false);
    }
}
