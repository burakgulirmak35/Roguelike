using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PoolTypes
{
    Enemy, Bullet, BloodShot, WorldTextPopup
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

    [Header("DamageText")]
    [SerializeField] private int WorldTextPoolCount;
    [SerializeField] private GameObject WorldTextPrefab;

    [Header("Holders")]
    [SerializeField] private Transform EnemyHolder;
    [SerializeField] private Transform BulletHolder;
    [SerializeField] private Transform BloodShotHolder;
    [SerializeField] private Transform WorldTextHolder;
    [Space]
    private Queue<GameObject> enemyPool = new Queue<GameObject>();
    private Queue<GameObject> bulletPool = new Queue<GameObject>();
    private Queue<GameObject> BloodShotPool = new Queue<GameObject>();
    private Queue<GameObject> worldTextPool = new Queue<GameObject>();
    private GameObject tempObject;

    public static PoolManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        GeneratePool(EnemyPrefab, EnemyPoolCount, enemyPool, EnemyHolder);
        GeneratePool(BulletPrefab, BulletPoolCount, bulletPool, BulletHolder);
        GeneratePool(WorldTextPrefab, WorldTextPoolCount, worldTextPool, WorldTextHolder);

        GeneratePool(BloodShotPrefabs, BloodShotPoolCount, BloodShotPool, BloodShotHolder);
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
                tempObject.SetActive(false);
                return tempObject;
            case PoolTypes.Bullet:
                tempObject = bulletPool.Dequeue();
                bulletPool.Enqueue(tempObject);
                tempObject.SetActive(false);
                return tempObject;
            case PoolTypes.WorldTextPopup:
                tempObject = worldTextPool.Dequeue();
                worldTextPool.Enqueue(tempObject);
                tempObject.SetActive(false);
                return tempObject;
            case PoolTypes.BloodShot:
                tempObject = BloodShotPool.Dequeue();
                BloodShotPool.Enqueue(tempObject);
                tempObject.SetActive(false);
                return tempObject;
            default:
                tempObject = null;
                return tempObject;
        }
    }
}
