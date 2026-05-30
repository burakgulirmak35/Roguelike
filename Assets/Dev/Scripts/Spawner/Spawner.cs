using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

[RequireComponent(typeof(PoolManager))]
public class Spawner : MonoBehaviour
{
    [Header("Unit")]
    [SerializeField][Range(10, 30)] public float MinEnemyDistanceToSpawn;
    [SerializeField][Range(20, 50)] public float UnitDissapearDistance;
    public bool UnitDissapearEnabled = true;
    public bool ShowEnemyHealth = true;
    private int AliveEnemyCount;
    [SerializeField][Range(0, 1)] public float spawnDelay;

    [Header("Enemy Count Scaling (5 Kademe)")]
    [SerializeField] private int[] EnemyCountStages;
    [SerializeField] private int[] StageLevelThresholds;
    [SerializeField] private int MaxAliveEnemyCount;
    [SerializeField] private float HealthScalePerLevel = 0.1f;
    [Space]
    [Header("Item")]
    [SerializeField][Range(1, 5)] public float ItemDropDistanceMin;
    [SerializeField][Range(1, 5)] public float ItemDropDistanceMax;
    [Space]
    private GameObject tempItem;
    public static Spawner Instance { get; private set; }

    public float EnemyHealthMultiplier { get; private set; } = 1f;
    private const float respawnDelay = 1f;
    private static readonly WaitForSeconds _waitPopup = new WaitForSeconds(1.5f);
    private WaitForSeconds _waitRespawn;
    private WaitForSeconds _waitSpawnDelay;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        GameEvents.OnDamageTaken += OnDamageTaken;
        GameEvents.OnLevelUp += OnLevelUp;
    }

    private void OnDisable()
    {
        GameEvents.OnDamageTaken -= OnDamageTaken;
        GameEvents.OnLevelUp -= OnLevelUp;
    }

    private void OnLevelUp(int level)
    {
        for (int i = StageLevelThresholds.Length - 1; i >= 0; i--)
        {
            if (level >= StageLevelThresholds[i])
            {
                AliveEnemyCount = Mathf.Min(EnemyCountStages[i], MaxAliveEnemyCount);
                EnemyHealthMultiplier = 1f + level * HealthScalePerLevel;
                return;
            }
        }
    }

    private void OnDamageTaken(float amount, Vector3 pos, bool isPlayer) =>
        WorldTextPopup(((int)amount).ToString(), pos, Color.red);

    public void StartGame()
    {
        OnLevelUp(PlayerData.Instance.level);
        _waitRespawn = new WaitForSeconds(respawnDelay);
        _waitSpawnDelay = new WaitForSeconds(spawnDelay);
        StartCoroutine(SpawnTimer());
    }

    private int SpawnPosID;
    private GameObject spawnee;
    private Enemy tempSpawnEnemy;
    private IEnumerator SpawnTimer()
    {
        while (true)
        {
            while (EnemyManager.Instance.Count < AliveEnemyCount)
            {
                spawnee = PoolManager.Instance.GetFromPool(PoolTypes.Enemy);
                if (spawnee.gameObject.activeSelf) break;
                spawnee.transform.position = GetSpawnPos();
                spawnee.TryGetComponent(out tempSpawnEnemy);
                tempSpawnEnemy.Reborn();
                spawnee.SetActive(true);
                yield return _waitSpawnDelay;
            }
            yield return _waitRespawn;
        }
    }

    private Vector3 GetSpawnPos()
    {
        SpawnPosID++;
        if (SpawnPosID >= Enviroment.Instance.CurrentCity.SpawnPoints.Count) { SpawnPosID = 0; }
        while (Vector3.Distance(Enviroment.Instance.CurrentCity.SpawnPoints[SpawnPosID].position, Player.Instance.PlayerTransform.position) < MinEnemyDistanceToSpawn)
        {
            SpawnPosID++;
            if (SpawnPosID >= Enviroment.Instance.CurrentCity.SpawnPoints.Count) { SpawnPosID = 0; }
        }
        return Enviroment.Instance.CurrentCity.SpawnPoints[SpawnPosID].position;
    }

    #region CreateText -----------------------
    private PopupText tempPopupText;
    private GameObject tempTextObject;
    public void WorldTextPopup(string text, Vector3 position, Color textColor)
    {
        tempTextObject = PoolManager.Instance.GetFromPool(PoolTypes.WorldTextPopup);
        tempTextObject.TryGetComponent(out tempPopupText);
        tempTextObject.transform.position = position;
        tempPopupText.txt_Popup.text = text;
        tempPopupText.txt_Popup.color = textColor;
        tempTextObject.SetActive(true);
        StartCoroutine(DisableObject(tempTextObject));
    }

    #endregion

    #region Drop -----------------------
    private float randomAngle;
    private Vector3 randomPos;
    private Vector3 RandomPos
    {
        get
        {
            randomAngle = Random.Range(0, 360);
            randomPos.x = Mathf.Sin(randomAngle) * randomDistance;
            randomPos.z = Mathf.Cos(randomAngle) * randomDistance;
            return randomPos;
        }
    }
    private float randomDistance
    {
        get
        {
            return Random.Range(ItemDropDistanceMin, ItemDropDistanceMax);
        }
    }

    private GameObject tempExperience;
    public void DropExperience(Vector3 _dropPos)
    {
        tempExperience = PoolManager.Instance.GetItemFromPool(ItemType.Experience);

        _dropPos.y += 1;
        tempExperience.transform.position = _dropPos;
        tempExperience.SetActive(true);
        tempExperience.transform.DOJump(_dropPos + RandomPos, 3, 1, 0.5f * randomDistance);
    }

    private GameObject tempDropItem;
    public void DropRandomItem(Vector3 _dropPos)
    {
        tempDropItem = PoolManager.Instance.GetItemFromPool((ItemType)Random.Range(1, 8));

        _dropPos.y += 1;
        tempDropItem.transform.position = _dropPos;
        tempDropItem.SetActive(true);
        tempDropItem.transform.DOJump(_dropPos + RandomPos, 3, 1, 0.5f * randomDistance);
    }

    #endregion

    #region SpawnAtPos -----------------------

    public void SpawnAtPos(PoolTypes _poolTypes, Vector3 _pos)
    {
        tempItem = PoolManager.Instance.GetFromPool(_poolTypes);
        tempItem.SetActive(false);
        tempItem.transform.position = _pos;
        tempItem.SetActive(true);
    }

    public void SpawnAtPos(ItemType _itemType, Vector3 _pos)
    {
        tempItem = PoolManager.Instance.GetItemFromPool(_itemType);
        tempItem.SetActive(false);
        tempItem.transform.position = _pos;
        tempItem.SetActive(true);
    }

    #endregion


    private IEnumerator DisableObject(GameObject go)
    {
        yield return _waitPopup;
        go.SetActive(false);
    }

}