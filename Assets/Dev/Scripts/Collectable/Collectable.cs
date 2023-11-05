using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public enum ItemType
{
    Experience,
}

public class Collectable : MonoBehaviour
{
    [SerializeField] private ItemType itemType;
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
        transform.parent = Player.Instance.transform;
        transform.DOLocalJump(new Vector3(0, 1, 0), 2, 1, 0.2f).OnComplete(() => OnCollected());
        myCollider.enabled = false;
    }


    private void OnCollected()
    {
        transform.parent = PoolManager.Instance.ExperienceHolder;
        Player.Instance.AddExperience(1);
        gameObject.SetActive(false);
    }
}
