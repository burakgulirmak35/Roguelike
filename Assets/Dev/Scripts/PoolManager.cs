using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [Header("BulletExplosion")]
    [SerializeField] private int BulletExplosionPoolCount;
    [SerializeField] private GameObject BulletExplosionPrefab;

    [Header("DamageText")]
    [SerializeField] private int WorldTextPoolCount;
    [SerializeField] private GameObject WorldTextPrefab;

    [Header("Experience")]
    [SerializeField] private int ExperiencePoolCount;
    [SerializeField] private GameObject ExperiencePrefab;

    [Header("Collectable")]
    [SerializeField] private int CollectableCount;
    [SerializeField] private GameObject CollectableBombPrefab;
    [SerializeField] private GameObject CollectableBoosterPrefab;
    [SerializeField] private GameObject CollectableHealthPrefab;
    [SerializeField] private GameObject CollectableHowerBoardPrefab;
    [SerializeField] private GameObject CollectableMagnetPrefab;
    [SerializeField] private GameObject CollectableSlowMotionPrefab;
    [SerializeField] private GameObject CollectableSpeedBoostPrefab;


    [Header("Holders")]
    [SerializeField] private Transform EnemyHolder;
    [SerializeField] private Transform BulletHolder;
    [SerializeField] private Transform BulletExplosionHolder;
    [SerializeField] private Transform BloodShotHolder;
    [SerializeField] private Transform WorldTextHolder;
    [SerializeField] public Transform ExperienceHolder;
    [SerializeField] public Transform CollectableHolder;
    [Space]
    private Queue<GameObject> enemyPool = new Queue<GameObject>();
    private Queue<GameObject> bulletPool = new Queue<GameObject>();
    private Queue<GameObject> bulletexplosionPool = new Queue<GameObject>();
    private Queue<GameObject> bloodShotPool = new Queue<GameObject>();
    private Queue<GameObject> worldTextPool = new Queue<GameObject>();
    private Queue<GameObject> experiencePool = new Queue<GameObject>();
    [Space]
    private Queue<GameObject> collectableBombPool = new Queue<GameObject>();
    private Queue<GameObject> collectableBoosterPool = new Queue<GameObject>();
    private Queue<GameObject> collectableHealthPool = new Queue<GameObject>();
    private Queue<GameObject> collectableHowerBoardPool = new Queue<GameObject>();
    private Queue<GameObject> collectableMagnetPool = new Queue<GameObject>();
    private Queue<GameObject> collectableSlowMotionPool = new Queue<GameObject>();
    private Queue<GameObject> collectableSpeedBoostPool = new Queue<GameObject>();
    [Space]
    private GameObject tempObject;

    public static PoolManager Instance { get; private set; }
    void Awake()
    {
        Instance = this;
    }

    public void GeneratePools()
    {
        GeneratePool(EnemyPrefab, EnemyPoolCount, enemyPool, EnemyHolder);
        GeneratePool(BulletPrefab, BulletPoolCount, bulletPool, BulletHolder);
        GeneratePool(BulletExplosionPrefab, BulletExplosionPoolCount, bulletexplosionPool, BulletExplosionHolder);
        GeneratePool(BloodShotPrefabs, BloodShotPoolCount, bloodShotPool, BloodShotHolder);
        GeneratePool(WorldTextPrefab, WorldTextPoolCount, worldTextPool, WorldTextHolder);

        GeneratePool(ExperiencePrefab, ExperiencePoolCount, experiencePool, ExperienceHolder);
        GeneratePool(CollectableBombPrefab, CollectableCount, collectableBombPool, CollectableHolder);
        GeneratePool(CollectableBoosterPrefab, CollectableCount, collectableBoosterPool, CollectableHolder);
        GeneratePool(CollectableHealthPrefab, CollectableCount, collectableHealthPool, CollectableHolder);
        GeneratePool(CollectableHowerBoardPrefab, CollectableCount, collectableHowerBoardPool, CollectableHolder);
        GeneratePool(CollectableMagnetPrefab, CollectableCount, collectableMagnetPool, CollectableHolder);
        GeneratePool(CollectableSlowMotionPrefab, CollectableCount, collectableSlowMotionPool, CollectableHolder);
        GeneratePool(CollectableSpeedBoostPrefab, CollectableCount, collectableSpeedBoostPool, CollectableHolder);
    }

    private void GeneratePool(GameObject prefab, int count, Queue<GameObject> pool, Transform holder)
    {
        for (int i = 0; i < count; i++)
        {
            tempObject = Instantiate(prefab, holder);
            tempObject.SetActive(false);
            pool.Enqueue(tempObject);
        }
    }

    private void GeneratePool(GameObject[] prefabs, int count, Queue<GameObject> pool, Transform holder)
    {
        for (int i = 0; i < count; i++)
        {
            tempObject = Instantiate(prefabs[Random.Range(0, prefabs.Length)], holder);
            tempObject.SetActive(false);
            pool.Enqueue(tempObject);
        }
    }

    private GameObject tempEnemy;
    private GameObject tempBullet;
    private GameObject tempBulletExplosion;
    private GameObject tempBloodShot;
    private GameObject tempWorldTextPopup;
    public GameObject GetFromPool(PoolTypes type)
    {
        switch (type)
        {
            case PoolTypes.Enemy:
                tempEnemy = enemyPool.Dequeue();
                enemyPool.Enqueue(tempEnemy);
                return tempEnemy;
            case PoolTypes.Bullet:
                tempBullet = bulletPool.Dequeue();
                bulletPool.Enqueue(tempBullet);
                return tempBullet;
            case PoolTypes.BulletExplosion:
                tempBulletExplosion = bulletexplosionPool.Dequeue();
                bulletexplosionPool.Enqueue(tempBulletExplosion);
                return tempBulletExplosion;
            case PoolTypes.BloodShot:
                tempBloodShot = bloodShotPool.Dequeue();
                bloodShotPool.Enqueue(tempBloodShot);
                return tempBloodShot;
            case PoolTypes.WorldTextPopup:
                tempWorldTextPopup = worldTextPool.Dequeue();
                worldTextPool.Enqueue(tempWorldTextPopup);
                return tempWorldTextPopup;
            default:
                tempObject = null;
                return tempObject;
        }
    }


    private GameObject tempExperience;
    private GameObject tempDropItem;
    public GameObject GetItemFromPool(ItemType type)
    {
        switch (type)
        {
            case ItemType.Experience:
                tempExperience = experiencePool.Dequeue();
                experiencePool.Enqueue(tempExperience);
                return tempExperience;
            case ItemType.Bomb:
                tempDropItem = collectableBombPool.Dequeue();
                collectableBombPool.Enqueue(tempDropItem);
                return tempDropItem;
            case ItemType.Booster:
                tempDropItem = collectableBoosterPool.Dequeue();
                collectableBoosterPool.Enqueue(tempDropItem);
                return tempDropItem;
            case ItemType.Health:
                tempDropItem = collectableHealthPool.Dequeue();
                collectableHealthPool.Enqueue(tempDropItem);
                return tempDropItem;
            case ItemType.HowerBoard:
                tempDropItem = collectableHowerBoardPool.Dequeue();
                collectableHowerBoardPool.Enqueue(tempDropItem);
                return tempDropItem;
            case ItemType.Magnet:
                tempDropItem = collectableMagnetPool.Dequeue();
                collectableMagnetPool.Enqueue(tempDropItem);
                return tempDropItem;
            case ItemType.SlowMotion:
                tempDropItem = collectableSlowMotionPool.Dequeue();
                collectableSlowMotionPool.Enqueue(tempDropItem);
                return tempDropItem;
            case ItemType.SpeedBoost:
                tempDropItem = collectableSpeedBoostPool.Dequeue();
                collectableSpeedBoostPool.Enqueue(tempDropItem);
                return tempDropItem;
            default:
                tempObject = null;
                return tempObject;
        }
    }
}
