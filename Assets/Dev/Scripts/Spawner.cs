using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(PoolManager))]
public class Spawner : MonoBehaviour
{
    [SerializeField][Range(10, 30)] private float minEnemyDistanceToSpawn;
    [SerializeField] private int AliveEnemyCount;
    [SerializeField] public Transform SpawnPointsParent;
    [SerializeField] private List<Transform> SpawnPoints = new List<Transform>();
    [HideInInspector] public KdTree<Transform> ActiveEnemies = new KdTree<Transform>();
    [Space]
    private List<Transform> EnemyList = new List<Transform>();
    private GameObject spawnee;
    private int SpawnPosID;
    private Player player;

    public static Spawner Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        player = FindObjectOfType<Player>();
    }

    private void Start()
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
                spawnee.SetActive(true);
                ActiveEnemies.Add(spawnee.transform);
                EnemyList.Add(spawnee.transform);

                if (SpawnPosID >= SpawnPoints.Count) { SpawnPosID = 0; }
                while (Vector3.Distance(SpawnPoints[SpawnPosID].position, player.transform.position) < minEnemyDistanceToSpawn)
                {
                    SpawnPosID++;
                    if (SpawnPosID >= SpawnPoints.Count) { SpawnPosID = 0; }
                }

                spawnee.transform.position = SpawnPoints[SpawnPosID].position;
                spawnee.SetActive(true);
                SpawnPosID++;
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(10f);
        }
    }

    public void DeadEnemy(Transform _enemy, float _time)
    {
        EnemyList.RemoveAt(EnemyList.IndexOf(_enemy));
        ActiveEnemies.Clear();
        ActiveEnemies.AddAll(EnemyList);
        StartCoroutine(DisableObject(_enemy.gameObject, _time));
    }

    //-------------------------------------------

    private GameObject tempObject;
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

    private IEnumerator DisableObject(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        go.SetActive(false);
    }

}