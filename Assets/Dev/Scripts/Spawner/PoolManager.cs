using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField] private int EnemyPoolCount;
    [SerializeField] private GameObject EnemyPrefab;

    [Header("Bullets")]
    [SerializeField] private int BulletPoolCount;
    [SerializeField] private GameObject BulletPrefab;

    [Header("BloodShot")]
    [SerializeField] private int BloodShotPoolCount;
    [SerializeField] private GameObject[] BloodShotPrefabs;

    [Header("BloodLake")]
    [SerializeField] private int BloodLakePoolCount;
    [SerializeField] private GameObject BloodLakePrefab;

    [Header("BulletExplosion")]
    [SerializeField] private int BulletExplosionPoolCount;
    [SerializeField] private GameObject BulletExplosionPrefab;

    [Header("DamageText")]
    [SerializeField] private int WorldTextPoolCount;
    [SerializeField] private GameObject WorldTextPrefab;

    [Header("Experience")]
    [SerializeField] private int ExperiencePoolCount;
    [SerializeField] private GameObject ExperiencePrefab;

    [Header("VFX")]
    [SerializeField] private int MegaExplosionPoolCount;
    [SerializeField] private GameObject MegaExplosionPrefab;
    [SerializeField] private int MagnetPoolCount;
    [SerializeField] private GameObject MagnetPrefab;
    [SerializeField] private int SimpleExplosionPoolCount;
    [SerializeField] private GameObject SimpleExplosionPrefab;
    [SerializeField] private int SlowAreaVFXPoolCount;
    [SerializeField] private GameObject SlowAreaVFXPrefab;

    [Header("Collectable")]
    [SerializeField] private int CollectableCount;
    [SerializeField] private GameObject CollectableBombPrefab;
    [SerializeField] private GameObject CollectableFireRateBoostPrefab;
    [SerializeField] private GameObject CollectableHealthPrefab;
    [SerializeField] private GameObject CollectableMeshTrainPrefab;
    [SerializeField] private GameObject CollectableMagnetPrefab;
    [SerializeField] private GameObject CollectableSlowAreaPrefab;
    [SerializeField] private GameObject CollectableSpeedBoostPrefab;

    [Header("Holders")]
    [SerializeField] private Transform EnemyHolder;
    [SerializeField] private Transform BulletHolder;
    [SerializeField] private Transform BulletExplosionHolder;
    [SerializeField] private Transform BloodSHolder;
    [SerializeField] private Transform WorldTextHolder;
    [SerializeField] private Transform VFXHolder;
    [SerializeField] public Transform ExperienceHolder;
    [SerializeField] public Transform CollectableHolder;

    // Enum index ile erişilen pool array'leri
    private Queue<GameObject>[] _pools;
    private Queue<GameObject>[] _itemPools;

    // Geliştirme sırasında takip için temp field'lar
    private GameObject tempEnemy;
    private GameObject tempBullet;
    private GameObject tempBulletExplosion;
    private GameObject tempBloodShot;
    private GameObject tempBloodLake;
    private GameObject tempWorldTextPopup;
    private GameObject tempVfx;
    private GameObject tempExperience;
    private GameObject tempDropItem;
    private GameObject tempObject;

    public static PoolManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public void GeneratePools()
    {
        _pools = new Queue<GameObject>[(int)PoolTypes._Count];
        _itemPools = new Queue<GameObject>[(int)ItemType._Count];

        CreatePool(PoolTypes.Enemy, EnemyPrefab, EnemyPoolCount, EnemyHolder);
        CreatePool(PoolTypes.Bullet, BulletPrefab, BulletPoolCount, BulletHolder);
        CreatePool(PoolTypes.BloodShot, BloodShotPrefabs, BloodShotPoolCount, BloodSHolder);
        CreatePool(PoolTypes.BloodLake, BloodLakePrefab, BloodLakePoolCount, BloodSHolder);
        CreatePool(PoolTypes.BulletExplosion, BulletExplosionPrefab, BulletExplosionPoolCount, BulletExplosionHolder);
        CreatePool(PoolTypes.WorldTextPopup, WorldTextPrefab, WorldTextPoolCount, WorldTextHolder);
        CreatePool(PoolTypes.SimpleExplosion, SimpleExplosionPrefab, SimpleExplosionPoolCount, VFXHolder);
        CreatePool(PoolTypes.MegaExplosion, MegaExplosionPrefab, MegaExplosionPoolCount, VFXHolder);
        CreatePool(PoolTypes.Magnet, MagnetPrefab, MagnetPoolCount, VFXHolder);
        CreatePool(PoolTypes.SlowAreaVFX, SlowAreaVFXPrefab, SlowAreaVFXPoolCount, VFXHolder);

        CreateItemPool(ItemType.Experience, ExperiencePrefab, ExperiencePoolCount, ExperienceHolder);
        CreateItemPool(ItemType.Bomb, CollectableBombPrefab, CollectableCount, CollectableHolder);
        CreateItemPool(ItemType.FireRateBoost, CollectableFireRateBoostPrefab, CollectableCount, CollectableHolder);
        CreateItemPool(ItemType.Health, CollectableHealthPrefab, CollectableCount, CollectableHolder);
        CreateItemPool(ItemType.MeshTrain, CollectableMeshTrainPrefab, CollectableCount, CollectableHolder);
        CreateItemPool(ItemType.Magnet, CollectableMagnetPrefab, CollectableCount, CollectableHolder);
        CreateItemPool(ItemType.SlowArea, CollectableSlowAreaPrefab, CollectableCount, CollectableHolder);
        CreateItemPool(ItemType.SpeedBoost, CollectableSpeedBoostPrefab, CollectableCount, CollectableHolder);
    }

    // -----------------------------------------------------------------------
    // Get
    // -----------------------------------------------------------------------

    public GameObject GetFromPool(PoolTypes type)
    {
        return Fetch(_pools[(int)type], type.ToString());
    }

    public GameObject GetItemFromPool(ItemType type)
    {
        return Fetch(_itemPools[(int)type], type.ToString());
    }

    // -----------------------------------------------------------------------
    // Core fetch — inactive arar, hepsi aktifse en eskiyi çalar
    // -----------------------------------------------------------------------

    private static GameObject Fetch(Queue<GameObject> pool, string label)
    {
        int count = pool.Count;
        for (int i = 0; i < count; i++)
        {
            var obj = pool.Dequeue();
            pool.Enqueue(obj);
            if (!obj.activeSelf) return obj;
        }

        Debug.LogWarning($"[Pool:{label}] Tüm objeler aktif — pool boyutu artırılmalı.");
        var stolen = pool.Dequeue();
        pool.Enqueue(stolen);
        return stolen;
    }

    // -----------------------------------------------------------------------
    // Pool oluşturma
    // -----------------------------------------------------------------------

    private void CreatePool(PoolTypes type, GameObject prefab, int count, Transform holder)
    {
        var pool = new Queue<GameObject>(count);
        for (int i = 0; i < count; i++)
        {
            tempObject = Instantiate(prefab, holder);
            tempObject.SetActive(false);
            pool.Enqueue(tempObject);
        }
        _pools[(int)type] = pool;
    }

    private void CreatePool(PoolTypes type, GameObject[] prefabs, int count, Transform holder)
    {
        var pool = new Queue<GameObject>(count);
        for (int i = 0; i < count; i++)
        {
            tempObject = Instantiate(prefabs[Random.Range(0, prefabs.Length)], holder);
            tempObject.SetActive(false);
            pool.Enqueue(tempObject);
        }
        _pools[(int)type] = pool;
    }

    private void CreateItemPool(ItemType type, GameObject prefab, int count, Transform holder)
    {
        var pool = new Queue<GameObject>(count);
        for (int i = 0; i < count; i++)
        {
            tempObject = Instantiate(prefab, holder);
            tempObject.SetActive(false);
            pool.Enqueue(tempObject);
        }
        _itemPools[(int)type] = pool;
    }
}
