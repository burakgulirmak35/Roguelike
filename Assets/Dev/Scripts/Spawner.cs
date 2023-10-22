using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(PoolManager))]
public class Spawner : MonoBehaviour
{
    public static Spawner Instance { get; private set; }
    private PoolManager poolManager;
    [SerializeField] private int SpawnCount;
    [SerializeField] private List<Transform> SpawnPositions = new List<Transform>();
    private GameObject spawnee;
    private int SpawnPosID;
    [HideInInspector] public KdTree<Transform> ActiveEnemies = new KdTree<Transform>();
    private List<Transform> EnemyList = new List<Transform>();

    private void Awake()
    {
        Instance = this;
        poolManager = GetComponent<PoolManager>();
    }

    private void Start()
    {
        StartCoroutine(SpawnTimer());
    }

    private IEnumerator SpawnTimer()
    {
        while (ActiveEnemies.Count < SpawnCount)
        {
            spawnee = poolManager.GetFromPool(PoolTypes.Enemy);
            spawnee.SetActive(true);
            ActiveEnemies.Add(spawnee.transform);
            EnemyList.Add(spawnee.transform);

            if (SpawnPosID >= SpawnPositions.Count) { SpawnPosID = 0; }
            spawnee.transform.position = SpawnPositions[SpawnPosID].position;
            spawnee.SetActive(true);
            SpawnPosID++;
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(180f);
    }

    public void DeadEnemy(Transform _enemy)
    {
        EnemyList.RemoveAt(EnemyList.IndexOf(_enemy));
        ActiveEnemies.Clear();
        ActiveEnemies.AddAll(EnemyList);
        _enemy.gameObject.SetActive(false);
    }

    //-------------------------------------------

    private GameObject tempObject;
    private TextMeshProUGUI tempText;
    public void WorldTextPopup(string text, Vector3 position, Color textColor)
    {
        tempObject = poolManager.GetFromPool(PoolTypes.WorldTextPopup);
        tempText = tempObject.GetComponent<PopupText>().txt_Popup;
        tempObject.transform.position = position;
        tempText.text = text;
        tempText.color = textColor;
        tempObject.SetActive(true);
        StartCoroutine(DisableObject(tempObject, 1.5f));
    }

    private IEnumerator DisableObject(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        go.SetActive(false);
    }

}