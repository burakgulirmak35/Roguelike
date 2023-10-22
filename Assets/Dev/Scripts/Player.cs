using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.Animations.Rigging;

public class Player : MonoBehaviour, IDamagable
{
    [SerializeField] private GunType SelectedGun;
    private Gun gun;

    [Header("Animations")]
    [SerializeField] private Animator PlayerAnim;

    [Header("Model")]
    [SerializeField] private Transform Body;
    [SerializeField] private GameObject Rifle;
    [Space]
    private NavMeshAgent Agent;
    private FloatingJoystick joystick;
    private Vector3 direction;

    [Header("Health")]
    [SerializeField] private HealthSystem healthSystem;

    [Header("Stats")]
    [SerializeField] private float speed;
    private GunSO gunSO;

    [Header("Aim")]
    [SerializeField] private Transform AimPoint;
    [SerializeField] private Transform DefaultAimPoint;
    [SerializeField] private TwoBoneIKConstraint LeftArmIK;
    [SerializeField] private Transform LeftArmTarget;

    private Vector3 TargetPoint;
    private Transform ClosestEnemy;
    private float DistanceToEnemy;

    [Header("State")]
    private bool Alive = true;
    private bool EnemyInRange = false;

    private void Awake()
    {
        joystick = FindObjectOfType<FloatingJoystick>();
        Agent = GetComponent<NavMeshAgent>();
        Agent.updateRotation = false;
        EquipGun(SelectedGun);
        healthSystem.SetHealth(100);
    }

    private void Start()
    {
        StartCoroutine(TakeAimLoop());
    }

    private void EquipGun(GunType _gunType)
    {
        if (gun != null)
        {
            gun.gameObject.SetActive(false);
        }

        switch (_gunType)
        {
            case GunType.Rifle:
                Rifle.SetActive(true);
                gun = Rifle.GetComponent<Gun>();
                PlayerAnim.SetBool("Rifle", true);
                break;
        }

        gunSO = gun.GetGunSO();
        LeftArmTarget.position = gun.GetLeftHandPos().position;
        LeftArmTarget.rotation = gun.GetLeftHandPos().rotation;
    }

    //Vertical yukarı aşağı
    private void Update()
    {
        direction = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        Agent.Move(direction * speed * Time.deltaTime);

        if (direction.magnitude > 0 && !EnemyInRange)
        {
            Body.forward = direction;
        }

        float velocityZ = Vector3.Dot(direction.normalized, Body.forward);
        float velocityX = Vector3.Dot(direction.normalized, Body.right);

        PlayerAnim.SetFloat("Speed", direction.magnitude * 2);
        PlayerAnim.SetFloat("Vertical", velocityZ);
        PlayerAnim.SetFloat("Horizontal", velocityX);
    }

    #region GunFire
    private IEnumerator TakeAimLoop()
    {
        while (Alive)
        {
            TakeAim();
            yield return new WaitForSeconds(0.1f);
        }
    }
    private void TakeAim()
    {
        Spawner.Instance.ActiveEnemies.UpdatePositions();
        if (Spawner.Instance.ActiveEnemies.Count > 0)
        {
            if (ClosestEnemy != null) ClosestEnemy.GetComponent<ITargetable>().Targeted(false);
            ClosestEnemy = Spawner.Instance.ActiveEnemies.FindClosest(transform.position);
            ClosestEnemy.GetComponent<ITargetable>().Targeted(true);
            DistanceToEnemy = Vector3.Distance(ClosestEnemy.position, transform.position);
            TargetPoint = ClosestEnemy.position + new Vector3(0, 2f, 0);

            if (DistanceToEnemy <= gunSO.FireRange)
            {
                EnemyInRange = true;
                PlayerAnim.SetBool("Aim", true);
                AimPoint.transform.position = new Vector3(TargetPoint.x, 1.1f, TargetPoint.z);
                Body.DOLookAt(new Vector3(TargetPoint.x, Body.position.y, TargetPoint.z), 0.1f);
                LeftArmIK.weight = 1;
                gun.StartFire();
            }
            else
            {
                gun.StopFire();
                EnemyInRange = false;
                PlayerAnim.SetBool("Aim", false);
                AimPoint.transform.position = DefaultAimPoint.position;
                LeftArmIK.weight = 0;
            }
        }
        else
        {
            gun.StopFire();
            EnemyInRange = false;
            PlayerAnim.SetBool("Aim", false);
            AimPoint.transform.position = DefaultAimPoint.position;
            LeftArmIK.weight = 0;
        }
    }
    #endregion

    #region HealthSystem
    public void TakeDamage(float damageTaken)
    {
        healthSystem.TakeDamage(damageTaken);
        Spawner.Instance.WorldTextPopup(((int)damageTaken).ToString(), transform.position, Color.red);
    }

    private void healthSystem_OnDead(object sender, System.EventArgs e)
    {

    }
    #endregion
}
