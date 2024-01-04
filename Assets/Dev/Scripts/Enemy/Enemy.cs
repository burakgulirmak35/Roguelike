using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class Enemy : MonoBehaviour, ITargetable
{
    [Header("HealtSystem")]
    [SerializeField] public EnemyHealthSystem enemyHealthSystem;
    [Header("Stats")]
    [SerializeField] private EnemySO enemySO;
    private float attackSpeed;
    [Header("Animations")]
    [SerializeField] private Animator EnemyAnim;
    private AnimEventController animEventController;
    [Header("Selected")]
    [SerializeField] private GameObject TargetedIcon;
    [Header("Model")]
    [SerializeField] private Transform Body;
    [Header("Collider")]
    private CapsuleCollider myCollider;
    [Header("AI")]
    public NavMeshAgent EnemyAgent;
    private bool isBusy;

    [Space]
    private Transform myTransform;
    private Vector3 tempPosition;

    private void Awake()
    {
        myCollider = GetComponent<CapsuleCollider>();
        //-----------------------------------------------
        EnemyAgent = GetComponent<NavMeshAgent>();
        EnemyAgent.speed = enemySO.Speed;
        EnemyAgent.stoppingDistance = enemySO.StartAttackRange;
        EnemyAgent.updateRotation = false;
        attackSpeed = enemySO.AttackSpeed;
        //-----------------------------------------------
        enemyHealthSystem.OnDead += OnDead;
        //-----------------------------------------------
        animEventController = EnemyAnim.GetComponent<AnimEventController>();
        animEventController.OnAttack += OnAttack;
        //-----------------------------------------------
        myTransform = this.transform;
    }

    public void Reborn()
    {
        StopDisable();
        enemyHealthSystem.isAlive = true;
        enemyHealthSystem.HealthBar.SetActive(false);
        TargetedIcon.SetActive(false);
        enemyHealthSystem.SetHealth(enemySO.Health);
        myCollider.enabled = true;

        EnemyAgent.speed = enemySO.Speed;
        attackSpeed = enemySO.AttackSpeed;
        EnemyAnim.SetFloat("Speed", 1);
    }

    void OnEnable()
    {
        EnemyAgent.enabled = true;
        EnemyManager.Instance.AddEnemy(this);
        EnemyAnim.SetBool("isAlive", true);
        EnemyAnim.Play("Idle");
        EnemyAnim.SetBool("canMove", true);
    }

    private float distance;
    public void FollowPlayer()
    {
        if (enemyHealthSystem.isAlive && !isBusy)
        {
            distance = DistToPlayer();
            if (distance > Spawner.Instance.UnitDissapearDistance)
            {
                Remove();
                return;
            }

            Body.DOLookAt(EnemyManager.Instance.TargetPos, 0.2f);

            if (distance <= enemySO.StartAttackRange)
            {
                StartAttack();
            }
            else
            {
                EnemyAgent.SetDestination(EnemyManager.Instance.TargetPos);
            }
        }
    }

    public void Targeted(bool state)
    {
        TargetedIcon.SetActive(state);
    }

    #region DeadAlive
    private void OnDead()
    {
        UIManager.Instance.AddScore();

        Spawner.Instance.DeadEnemy(transform);
        EnemyAgent.ResetPath();
        EnemyAgent.enabled = false;

        StartDisable();
        enemyHealthSystem.HideHealth();

        DropExperience();
        DropBloodPool();
        DropItem();

        TargetedIcon.SetActive(false);
        EnemyAnim.Play("Dead " + Random.Range(0, 4).ToString());
        myCollider.enabled = false;
        EnemyAnim.SetBool("isAlive", false);
    }

    private void DropBloodPool()
    {
        tempPosition = myTransform.position;
        tempPosition.y = 0.1f;
        Spawner.Instance.SpawnAtPos(PoolTypes.BloodLake, tempPosition);
    }

    private void DropExperience()
    {
        for (int i = 0; i < enemySO.ExperienceAmount; i++)
        {
            Spawner.Instance.DropExperience(myTransform.position);
        }
    }

    private void DropItem()
    {
        if (enemySO.DropRate >= Random.Range(1, 101))
            Spawner.Instance.DropRandomItem(myTransform.position);
    }
    // fast remove
    public void Remove()
    {
        EnemyManager.Instance.DeadEnemy(this);
        EnemyAgent.ResetPath();
        EnemyAgent.enabled = false;

        enemyHealthSystem.HideHealth();
        myCollider.enabled = false;
        enemyHealthSystem.isAlive = false;
        EnemyAnim.SetBool("isAlive", false);
        gameObject.SetActive(false);
    }
    #endregion

    #region  Attack
    private float DistToPlayer()
    {
        return Vector3.Distance(Player.Instance.PlayerTransform.position, myTransform.position);
    }

    private int RandomAttackAnimationIndex()
    {
        return Random.Range(0, 4);
    }

    private void StartAttack()
    {
        if (!enemyHealthSystem.isAlive) return;

        isBusy = true;
        EnemyAgent.ResetPath();

        // vuruş animasyonları 2 saniye sürüyor hepsi aynı süre
        EnemyAnim.SetFloat("AttackSpeed", attackSpeed);
        EnemyAnim.Play("Attack" + RandomAttackAnimationIndex().ToString());
        if (AttackCoro != null) { StopCoroutine(AttackCoro); }
        AttackCoro = StartCoroutine(AttackTimer());
    }

    public void OnAttack()
    {
        distance = DistToPlayer();
        if (distance <= enemySO.AttackRange)
        {
            Player.Instance.healthSystem.TakeDamage(enemySO.Damage);
        }
    }
    private Coroutine AttackCoro;
    private IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(1f);
        isBusy = false;
    }

    #endregion

    #region  DisableTimer
    private Coroutine DisableCoro;
    private void StartDisable()
    {
        if (DisableCoro != null)
        {
            StopCoroutine(DisableCoro);
        }
        DisableCoro = StartCoroutine(DisableTimer());
    }
    private void StopDisable()
    {
        if (DisableCoro != null)
        {
            StopCoroutine(DisableCoro);
        }
    }
    private IEnumerator DisableTimer()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }
    #endregion


    #region  Slow

    private Coroutine SlowDownCoro;
    public void SlowDown(float _percent, float _time)
    {
        EnemyAgent.speed = enemySO.Speed * _percent;
        attackSpeed = enemySO.AttackSpeed * _percent;
        EnemyAnim.SetFloat("Speed", 1 * _percent);

        if (SlowDownCoro != null) StopCoroutine(SlowDownCoro);
        SlowDownCoro = StartCoroutine(SlowDownTimer(_time));
    }
    private IEnumerator SlowDownTimer(float _time)
    {
        yield return new WaitForSeconds(_time);

        EnemyAgent.speed = enemySO.Speed;
        attackSpeed = enemySO.AttackSpeed;
        EnemyAnim.SetFloat("Speed", 1);
    }

    #endregion
}
