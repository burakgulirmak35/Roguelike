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
    [Header("Collider")]
    private CapsuleCollider myCollider;
    [Header("AI")]
    private NavMeshAgent EnemyAgent;
    private int RandomAnimationIndex;

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

    private void Awake()
    {
        myCollider = GetComponent<CapsuleCollider>();
        //-----------------------------------------------
        EnemyAgent = GetComponent<NavMeshAgent>();
        EnemyAgent.speed = enemySO.Speed;
        EnemyAgent.stoppingDistance = enemySO.StartAttackRange;
        EnemyAgent.updateRotation = false;
        //-----------------------------------------------
        healthSystem.OnDead += OnDead;
        //-----------------------------------------------
        animEventController = EnemyAnim.GetComponent<AnimEventController>();
        animEventController.OnAttack += OnAttack;
        animEventController.OnAttackEnd += OnAttackEnd;
    }

    public void Reborn()
    {
        StopDisable();
        isAlive = true;
        HealthBar.SetActive(false);
        TargetedIcon.SetActive(false);
        healthSystem.SetHealth(enemySO.Health);
        myCollider.enabled = true;
    }

    void OnEnable()
    {
        StartFollow();
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
    }

    private float distance;
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
                EnemyAgent.SetDestination(Player.Instance.transform.position);
                Body.DOLookAt(new Vector3(Player.Instance.transform.position.x, transform.position.y, Player.Instance.transform.position.z), 0.2f);
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
        StartDisable();
        HideHeath();
        DropExperience();
        EnemyAgent.ResetPath();

        Spawner.Instance.DeadEnemy(transform);
        RandomAnimationIndex = Random.Range(0, 4);
        EnemyAnim.Play("Death" + RandomAnimationIndex.ToString());
        myCollider.enabled = false;
        isAlive = false;
    }

    public void DropExperience()
    {
        for (int i = 0; i < enemySO.ExperienceAmount; i++)
        {
            Spawner.Instance.DropExperience(transform.position);
        }
    }

    // fast remove
    public void Remove()
    {
        StopFollow();
        HideHeath();

        EnemyAgent.ResetPath();
        Spawner.Instance.DeadEnemy(transform);
        myCollider.enabled = false;
        isAlive = false;

        gameObject.SetActive(false);
    }

    private void HideHeath()
    {
        if (ShowHealthCoro != null)
        {
            StopCoroutine(ShowHealthCoro);
            ShowHealthCoro = null;
            HealthBar.SetActive(false);
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
        HealthBar.SetActive(true);
        yield return new WaitForSeconds(10f);
        HealthBar.SetActive(false);
    }
    #endregion

    #region  Attack
    private float DistToPlayer()
    {
        return Vector3.Distance(Player.Instance.transform.position, transform.position);
    }

    private void StartAttack()
    {
        if (!isAlive) return;

        StopFollow();
        EnemyAgent.ResetPath();

        EnemyAnim.SetFloat("AttackSpeed", 1);
        RandomAnimationIndex = Random.Range(0, 4);
        EnemyAnim.Play("Attack" + RandomAnimationIndex.ToString());
    }

    public void OnAttack()
    {
        distance = DistToPlayer();
        if (distance <= enemySO.AttackRange)
        {
            Player.Instance.TakeDamage(enemySO.Damage);
        }
    }

    public void OnAttackEnd()
    {
        if (DistToPlayer() <= enemySO.StartAttackRange)
        {
            StartAttack();
        }
        else
        {
            StartFollow();
        }
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
