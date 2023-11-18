using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class Enemy : MonoBehaviour, IDamagable, ITargetable
{
    [Header("HealtSystem")]
    [SerializeField] private BasicHealthSystem basicHealthSystem;
    [Header("Stats")]
    [SerializeField] private EnemySO enemySO;
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
    private NavMeshAgent EnemyAgent;
    private int RandomAnimationIndex;
    private bool isAlive;

    [Space]
    private Player player;
    private Transform playerTransform;
    private Transform myTransform;

    private void Awake()
    {
        myCollider = GetComponent<CapsuleCollider>();
        //-----------------------------------------------
        EnemyAgent = GetComponent<NavMeshAgent>();
        EnemyAgent.speed = enemySO.Speed;
        EnemyAgent.stoppingDistance = enemySO.StartAttackRange;
        EnemyAgent.updateRotation = false;
        //-----------------------------------------------
        basicHealthSystem.OnDead += OnDead;
        //-----------------------------------------------
        animEventController = EnemyAnim.GetComponent<AnimEventController>();
        animEventController.OnAttack += OnAttack;
        //-----------------------------------------------
        player = FindObjectOfType<Player>();
        playerTransform = player.transform;
        myTransform = this.transform;
    }

    public void Reborn()
    {
        StopDisable();
        isAlive = true;
        basicHealthSystem.HealthBar.SetActive(false);
        TargetedIcon.SetActive(false);
        basicHealthSystem.SetHealth(enemySO.Health);
        myCollider.enabled = true;
    }

    void OnEnable()
    {
        StartFollow();
        EnemyAnim.SetBool("isAlive", true);
        EnemyAnim.Play("Idle");
        EnemyAnim.SetBool("canMove", true);
    }

    private Coroutine FollowCorotine;
    private void StartFollow()
    {
        if (FollowCorotine != null)
        {
            StopCoroutine(FollowCorotine);
        }
        FollowCorotine = StartCoroutine(FollowTimer());
    }
    private void StopFollow()
    {
        if (FollowCorotine != null)
        {
            StopCoroutine(FollowCorotine);
        }
        EnemyAgent.ResetPath();
    }

    private float distance;
    private Vector3 lookPos;
    private IEnumerator FollowTimer()
    {
        while (isAlive)
        {
            yield return new WaitForSeconds(0.2f);
            distance = DistToPlayer();
            if (distance <= enemySO.StartAttackRange) { StartAttack(); }
            else if (distance > Spawner.Instance.UnitDissapearDistance) { Remove(); }
            else
            {
                lookPos = playerTransform.position;
                lookPos.y = myTransform.position.y;
                EnemyAgent.SetDestination(playerTransform.position);
                Body.DOLookAt(lookPos, 0.2f);
            }
        }
    }

    public void Targeted(bool state)
    {
        TargetedIcon.SetActive(state);
    }

    #region HealthSystem
    public void TakeDamage(float damageTaken)
    {
        if (!isAlive) { return; }
        ShowHealth();
        basicHealthSystem.TakeDamage(damageTaken);
        Spawner.Instance.WorldTextPopup(((int)damageTaken).ToString(), myTransform.position, Color.red);

    }

    private void OnDead()
    {
        StopFollow();
        StartDisable();
        HideHealth();
        DropExperience();

        TargetedIcon.SetActive(false);
        Spawner.Instance.DeadEnemy(transform);
        RandomAnimationIndex = Random.Range(0, 4);
        EnemyAnim.Play("Death" + RandomAnimationIndex.ToString());
        myCollider.enabled = false;
        isAlive = false;
        EnemyAnim.SetBool("isAlive", false);
    }

    public void DropExperience()
    {
        for (int i = 0; i < enemySO.ExperienceAmount; i++)
        {
            Spawner.Instance.DropExperience(myTransform.position);
        }
    }

    // fast remove
    public void Remove()
    {
        StopFollow();
        HideHealth();

        Spawner.Instance.DeadEnemy(transform);
        myCollider.enabled = false;
        isAlive = false;
        EnemyAnim.SetBool("isAlive", false);
        gameObject.SetActive(false);
    }

    private void HideHealth()
    {
        if (ShowHealthCoro != null)
        {
            StopCoroutine(ShowHealthCoro);
            ShowHealthCoro = null;
            basicHealthSystem.HealthBar.SetActive(false);
        }
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
        basicHealthSystem.HealthBar.SetActive(true);
        yield return new WaitForSeconds(10f);
        basicHealthSystem.HealthBar.SetActive(false);
    }
    #endregion

    #region  Attack
    private float DistToPlayer()
    {
        return Vector3.Distance(playerTransform.position, myTransform.position);
    }

    private void StartAttack()
    {
        if (!isAlive) return;

        StopFollow();

        // vuruş animasyonları 2 saniye
        EnemyAnim.SetFloat("AttackSpeed", 2);
        RandomAnimationIndex = Random.Range(0, 4);
        EnemyAnim.Play("Attack" + RandomAnimationIndex.ToString());
        if (AttackCoro != null) { StopCoroutine(AttackCoro); }
        AttackCoro = StartCoroutine(AttackTimer());
    }

    public void OnAttack()
    {
        distance = DistToPlayer();
        if (distance <= enemySO.AttackRange)
        {
            Player.Instance.TakeDamage(enemySO.Damage);
        }
    }
    private Coroutine AttackCoro;
    private IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(1f);
        StartFollow();
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
}
