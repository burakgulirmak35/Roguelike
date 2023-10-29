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
    [Header("Stats")]
    [SerializeField] private EnemySO enemySO;
    [Header("Animations")]
    [SerializeField] private Animator EnemyAnim;
    private AnimEventController animEventController;
    [Header("Selected")]
    [SerializeField] private GameObject TargetedIcon;
    [Header("Model")]
    [SerializeField] private Transform Body;
    [Header("AI")]
    private NavMeshAgent EnemyAgent;


    private int RandomAnimationIndex;
    private Transform playerTransform;
    private Player player;
    private bool isAttacking;

    private bool isAlive
    {
        get { return healthSystem.isAlive; }
        set
        {
            if (value)
            {
                healthSystem.isAlive = true;
                EnemyAnim.SetBool("isAlive", true);
            }
            else
            {
                healthSystem.isAlive = false;
                EnemyAnim.SetBool("isAlive", false);
            }
        }
    }

    private bool _canMove;
    private bool canMove
    {
        get { return _canMove; }
        set
        {
            if (value)
            {
                if (isAlive) { _canMove = true; }
                else { _canMove = false; }
                EnemyAnim.SetBool("canMove", _canMove);
            }
            else
            {
                EnemyAgent.ResetPath();
                _canMove = false;
            }
        }
    }

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        playerTransform = player.transform;
        //-----------------------------------------------
        EnemyAgent = GetComponent<NavMeshAgent>();
        EnemyAgent.speed = enemySO.Speed;
        EnemyAgent.updateRotation = false;
        //-----------------------------------------------
        healthSystem.OnDead += OnDead;
        //-----------------------------------------------
        animEventController = EnemyAnim.GetComponent<AnimEventController>();
        animEventController.OnAttack += OnAttack;
        animEventController.OnAttackEnd += OnAttackEnd;
    }

    private void OnEnable()
    {
        canMove = true;
        isAlive = true;
        HealthBar.SetActive(false);
        TargetedIcon.SetActive(false);
        healthSystem.SetHealth(100);
        EnemyAnim.Play("Idle");
        StartFollow();
    }

    private Coroutine FollowCorotine;
    private void StartFollow()
    {
        if (FollowCorotine != null)
        {
            StopCoroutine(FollowCorotine);
            FollowCorotine = null;
        }
        FollowCorotine = StartCoroutine(FollowTimer());
    }
    private void StopFollow()
    {
        if (FollowCorotine != null)
        {
            StopCoroutine(FollowCorotine);
            FollowCorotine = null;
        }
    }
    private IEnumerator FollowTimer()
    {
        while (isAlive)
        {
            yield return new WaitForSeconds(0.2f);
            if (DistToPlayer() <= EnemyAgent.stoppingDistance) { StartAttack(); }
            if (canMove)
            {
                EnemyAgent.SetDestination(playerTransform.position);
                Body.DOLookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z), 0.2f);
            }
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

    private void OnDead()
    {
        StopFollow();
        if (ShowHealthCoro != null)
        {
            StopCoroutine(ShowHealthCoro);
            ShowHealthCoro = null;
            HealthBar.SetActive(false);
        }

        Spawner.Instance.DeadEnemy(transform, 2f);
        RandomAnimationIndex = Random.Range(0, 4);
        EnemyAnim.Play("Death" + RandomAnimationIndex.ToString());
        isAlive = false;
        canMove = false;
    }

    private void ShowHealth()
    {
        if (ShowHealthCoro != null)
        {
            StopCoroutine(ShowHealthCoro);
            ShowHealthCoro = null;
        }

        if (isAlive)
        {
            ShowHealthCoro = StartCoroutine(ShowHealthTimer());
        }
    }

    private Coroutine ShowHealthCoro;
    private IEnumerator ShowHealthTimer()
    {
        HealthBar.SetActive(true);
        yield return new WaitForSeconds(30f);
        HealthBar.SetActive(false);
    }
    #endregion

    #region  Attack
    private float DistToPlayer()
    {
        return Vector3.Distance(playerTransform.position, transform.position);
    }

    private void StartAttack()
    {
        if (isAttacking || !isAlive) return;

        isAttacking = true;
        canMove = false;

        EnemyAnim.SetFloat("AttackSpeed", 1);
        RandomAnimationIndex = Random.Range(0, 4);
        EnemyAnim.Play("Attack" + RandomAnimationIndex.ToString());
    }

    private void DamageArea()
    {
        if (DistToPlayer() <= EnemyAgent.stoppingDistance)
        {
            player.TakeDamage(enemySO.Damage);
        }
    }

    public void OnAttack()
    {
        DamageArea();
    }

    public void OnAttackEnd()
    {
        isAttacking = false;
        canMove = true;
    }
    #endregion
}
