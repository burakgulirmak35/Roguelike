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
    [SerializeField] public int AliveEnemyCount;
    [SerializeField][Range(0, 1)] public float spawnDelay;
    [Space]
    [SerializeField] public Transform SpawnPointsParent;
    [HideInInspector] public KdTree<Transform> ActiveEnemies = new KdTree<Transform>();
    [Header("Item")]
    [SerializeField][Range(1, 5)] public float ItemDropDistanceMin;
    [SerializeField][Range(1, 5)] public float ItemDropDistanceMax;
    [Space]
    private List<Transform> SpawnPoints = new List<Transform>();
    private List<Transform> EnemyList = new List<Transform>();
    private GameObject spawnee;
    private GameObject tempObject;
    private int SpawnPosID;
    public static Spawner Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < SpawnPointsParent.childCount; i++)
        {
            SpawnPoints.Add(SpawnPointsParent.GetChild(i));
        }
    }

    public void StartGame()
    {
        StartCoroutine(SpawnTimer());
    }

    private IEnumerator SpawnTimer()
    {
        while (true)
        {
            while (ActiveEnemies.Count < AliveEnemyCount)
            {
                spawnee = PoolManager.Instance.GetFromPool(PoolTypes.Enemy);
                if (spawnee.gameObject.activeSelf) break;
                ActiveEnemies.Add(spawnee.transform);
                EnemyList.Add(spawnee.transform);
                spawnee.transform.position = GetSpawnPos();
                spawnee.GetComponent<Enemy>().Reborn();
                spawnee.SetActive(true);
                yield return new WaitForSeconds(spawnDelay);
            }
            yield return new WaitForSeconds(10f);
        }
    }

    private Vector3 GetSpawnPos()
    {
        SpawnPosID++;
        if (SpawnPosID >= SpawnPoints.Count) { SpawnPosID = 0; }
        while (Vector3.Distance(SpawnPoints[SpawnPosID].position, Player.Instance.PlayerTransform.position) < MinEnemyDistanceToSpawn)
        {
            SpawnPosID++;
            if (SpawnPosID >= SpawnPoints.Count) { SpawnPosID = 0; }
        }
        return SpawnPoints[SpawnPosID].position;
    }

    public void DeadEnemy(Transform _enemy)
    {
        EnemyList.Remove(_enemy);
        ActiveEnemies.Clear();
        ActiveEnemies.AddAll(EnemyList);
        ActiveEnemies.UpdatePositions();
    }

    #region CreateText -----------------------
    private TextMeshProUGUI tempText;
    public void WorldTextPopup(string text, Vector3 position, Color textColor)
    {
        tempObject = PoolManager.Instance.GetFromPool(PoolTypes.WorldTextPopup);
        tempText = tempObject.GetComponent<PopupText>().txt_Popup;
        tempObject.transform.position = position;
        tempText.text = text;
        tempText.color = textColor;
        tempObject.SetActive(true);
        StartCoroutine(DisableObject(tempObject, 1.5f));
    }

    #endregion

    #region Exp -----------------------
    private Vector3 randomPos;
    private float randomAngle;
    private float dropDistance;

    public void DropExperience(Vector3 _dropPos)
    {
        _dropPos.y += 1;
        dropDistance = Random.Range(ItemDropDistanceMin, ItemDropDistanceMax);

        randomAngle = Random.Range(0, 360);
        randomPos.x = Mathf.Sin(randomAngle) * dropDistance;
        randomPos.z = Mathf.Cos(randomAngle) * dropDistance;

        tempObject = PoolManager.Instance.GetFromPool(PoolTypes.Experience);
        tempObject.transform.position = _dropPos;
        tempObject.SetActive(true);
        tempObject.transform.DOJump(_dropPos + randomPos, 3, 1, 0.5f * dropDistance);
    }
    #endregion

    private IEnumerator DisableObject(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        go.SetActive(false);
    }

}