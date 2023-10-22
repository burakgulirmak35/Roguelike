using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class Enemy : MonoBehaviour, IDamagable, ITargetable
{
    [Header("HealtSystem")]
    [SerializeField] private GameObject HealthBar;
    [SerializeField] private HealthSystem healthSystem;
    private bool isAlive;
    [Header("Stats")]
    [SerializeField] private EnemySO enemySO;
    [Header("Selected")]
    [SerializeField] private GameObject TargetedIcon;
    [Header("Model")]
    [SerializeField] private Transform Body;
    [Header("AI")]
    NavMeshAgent EnemyAgent;
    private Transform PlayerTransform;
    private Coroutine FollowCorotine;
    private Player player;

    private void Awake()
    {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //-----------------------------------------------
        EnemyAgent = GetComponent<NavMeshAgent>();
        EnemyAgent.speed = enemySO.Speed;
        EnemyAgent.updateRotation = false;
        //-----------------------------------------------
        healthSystem.OnDead += healthSystem_OnDead;
    }

    private void OnEnable()
    {
        isAlive = true;
        HealthBar.SetActive(false);
        TargetedIcon.SetActive(false);
        healthSystem.SetHealth(100);
        if (FollowCorotine != null)
        {
            StopCoroutine(FollowCorotine);
            FollowCorotine = null;
        }
        FollowCorotine = StartCoroutine(FollowTimer());
    }

    private IEnumerator FollowTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            EnemyAgent.SetDestination(PlayerTransform.position);
            Body.DOLookAt(new Vector3(PlayerTransform.position.x, transform.position.y, PlayerTransform.position.z), 0.2f);
        }
    }

    // icon under unity will open if targeted
    public void Targeted(bool state)
    {
        TargetedIcon.SetActive(state);
    }

    #region HealthSystem
    public void TakeDamage(float damageTaken)
    {
        if (!isAlive) { return; }
        ShowHealth();
        healthSystem.TakeDamage(damageTaken);
        Spawner.Instance.WorldTextPopup(((int)damageTaken).ToString(), transform.position, Color.red);

    }

    private void healthSystem_OnDead(object sender, System.EventArgs e)
    {
        if (FollowCorotine != null)
        {
            StopCoroutine(FollowCorotine);
            FollowCorotine = null;
        }
        if (ShowHeathCoro != null)
        {
            StopCoroutine(ShowHeathCoro);
            ShowHeathCoro = null;
        }
        Spawner.Instance.DeadEnemy(transform);
        isAlive = false;
    }

    private void ShowHealth()
    {
        if (ShowHeathCoro != null)
        {
            StopCoroutine(ShowHeathCoro);
            ShowHeathCoro = null;
        }
        ShowHeathCoro = StartCoroutine(ShowHealthTimer());
    }

    private Coroutine ShowHeathCoro;
    private IEnumerator ShowHealthTimer()
    {
        HealthBar.SetActive(true);
        yield return new WaitForSeconds(30f);
        HealthBar.SetActive(false);
    }
    #endregion



    #region  Attack --Test--
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                if (other.gameObject.GetComponent<IDamagable>() != null)
                {
                    other.gameObject.GetComponent<IDamagable>().TakeDamage(enemySO.Damage);
                }
                break;
            default:
                break;
        }
    }
    #endregion
}
