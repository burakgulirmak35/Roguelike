using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyManager : MonoBehaviour
{
    private List<Enemy> enemies = new List<Enemy>();
    [HideInInspector] public Vector3 TargetPos;

    public static EnemyManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(FollowPlayerTimer());
    }

    public void AddEnemy(Enemy _enemy)
    {
        enemies.Add(_enemy);
    }

    public void DeadEnemy(Enemy _enemy)
    {
        enemies.Remove(_enemy);
        Spawner.Instance.DeadEnemy(_enemy.transform);
    }

    private IEnumerator FollowPlayerTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            TargetPos = Player.Instance.PlayerTransform.position;
            TargetPos.y = 0;

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].FollowPlayer();
            }
        }
    }

}
