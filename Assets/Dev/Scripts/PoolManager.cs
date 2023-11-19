using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PoolTypes
{
    Enemy, Bullet, BulletExplosion, BloodShot, WorldTextPopup, Experience
}

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


    [Header("Holders")]
    [SerializeField] private Transform EnemyHolder;
    [SerializeField] private Transform BulletHolder;
    [SerializeField] private Transform BulletExplosionHolder;
    [SerializeField] private Transform BloodShotHolder;
    [SerializeField] private Transform WorldTextHolder;
    [SerializeField] public Transform ExperienceHolder;
    [Space]
    private Queue<GameObject> enemyPool = new Queue<GameObject>();
    private Queue<GameObject> bulletPool = new Queue<GameObject>();
    private Queue<GameObject> bulletexplosionPool = new Queue<GameObject>();
    private Queue<GameObject> bloodShotPool = new Queue<GameObject>();
    private Queue<GameObject> worldTextPool = new Queue<GameObject>();
    private Queue<GameObject> experiencePool = new Queue<GameObject>();
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

    public GameObject GetFromPool(PoolTypes type)
    {
        switch (type)
        {
            case PoolTypes.Enemy:
                tempObject = enemyPool.Dequeue();
                enemyPool.Enqueue(tempObject);
                return tempObject;
            case PoolTypes.Bullet:
                tempObject = bulletPool.Dequeue();
                bulletPool.Enqueue(tempObject);
                return tempObject;
            case PoolTypes.BulletExplosion:
                tempObject = bulletexplosionPool.Dequeue();
                bulletexplosionPool.Enqueue(tempObject);
                return tempObject;
            case PoolTypes.BloodShot:
                tempObject = bloodShotPool.Dequeue();
                bloodShotPool.Enqueue(tempObject);
                return tempObject;
            case PoolTypes.WorldTextPopup:
                tempObject = worldTextPool.Dequeue();
                worldTextPool.Enqueue(tempObject);
                return tempObject;
            case PoolTypes.Experience:
                tempObject = experiencePool.Dequeue();
                experiencePool.Enqueue(tempObject);
                return tempObject;
            default:
                tempObject = null;
                return tempObject;
        }
    }
}
