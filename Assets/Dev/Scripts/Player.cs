using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour, IDamagable
{
    [Header("Gun")]
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
    [SerializeField] public HealthSystem healthSystem;

    [Header("Level")]
    [SerializeField] private Slider slider_Exp;
    [SerializeField] private TextMeshProUGUI txt_Level;
    [SerializeField] private GameObject particle_LevelUp;
    private float expAmount;

    [Header("Aim")]
    [SerializeField] private Transform AimPoint;
    [SerializeField] private Transform DefaultAimPoint;
    [SerializeField] private Transform LeftArmTarget;
    [Space]
    [SerializeField] private TwoBoneIKConstraint LeftArmIK;
    [SerializeField] private MultiAimConstraint RightArmIK;
    [SerializeField] private MultiAimConstraint BodyIK;

    private Vector3 TargetPoint;
    private Transform ClosestEnemy;
    private float DistanceToEnemy;

    [Header("State")]
    private bool Alive = true;
    private bool EnemyInRange = false;
    private bool isAutoAim = false;

    [Space]
    [HideInInspector] public Transform PlayerTransform;

    public static Player Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        joystick = FindObjectOfType<FloatingJoystick>();
        Agent = GetComponent<NavMeshAgent>();

        Agent.updateRotation = false;
        PlayerTransform = this.transform;

        healthSystem.OnDead += OnDead;
    }

    public void StartGame()
    {
        PlayerData.Instance.LoadData();
        EquipGun(PlayerData.Instance.SelectedGun);
        healthSystem.SetHealth(PlayerData.Instance.MaxHealth);

        StartCoroutine(TakeAimLoop());
        AddExperience(0);
    }

    public void CheckUpgrades()
    {
        //EquipGun(PlayerData.Instance.SelectedGun);
        healthSystem.UpgradeMaxHealth(PlayerData.Instance.MaxHealth);
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
                break;
        }
        LeftArmTarget.position = gun.GetLeftHandPos().position;
        LeftArmTarget.rotation = gun.GetLeftHandPos().rotation;
    }

    //Vertical yukarı aşağı
    private void Update()
    {
        direction.x = joystick.Horizontal;
        direction.z = joystick.Vertical;
        Agent.Move(direction * PlayerData.Instance.MovementSpeed * Time.deltaTime);

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
    public void EnableAutoAim(bool state)
    {
        isAutoAim = state;
    }

    #region ---AutoAim---
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
            if (ClosestEnemy != null)
            {
                ClosestEnemy.GetComponent<ITargetable>().Targeted(false);
            }

            ClosestEnemy = Spawner.Instance.ActiveEnemies.FindClosest(PlayerTransform.position);
            ClosestEnemy.GetComponent<ITargetable>().Targeted(true);
            DistanceToEnemy = Vector3.Distance(ClosestEnemy.position, PlayerTransform.position);

            TargetPoint = ClosestEnemy.position;
            TargetPoint.y += DefaultAimPoint.position.y;

            if (DistanceToEnemy <= PlayerData.Instance.FireRange)
            {
                EnemyInRange = true;
                PlayerAnim.SetBool("Aim", true);
                AimPoint.position = TargetPoint;

                TargetPoint.y = Body.position.y;
                Body.DOLookAt(TargetPoint, 0.1f);

                LeftArmIK.weight = 1;
                RightArmIK.weight = 1;
                BodyIK.weight = 1;
                gun.StartFire();
            }
            else
            {
                gun.StopFire();
                EnemyInRange = false;
                PlayerAnim.SetBool("Aim", false);
                AimPoint.position = DefaultAimPoint.position;

                LeftArmIK.weight = 0;
                RightArmIK.weight = 0;
                BodyIK.weight = 0;
            }
        }
        else
        {
            gun.StopFire();
            EnemyInRange = false;
            PlayerAnim.SetBool("Aim", false);
            AimPoint.position = DefaultAimPoint.position;

            LeftArmIK.weight = 0;
            RightArmIK.weight = 0;
            BodyIK.weight = 0;
        }
    }
    #endregion

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Gate":

                break;
            case "Collectable":
                other.GetComponent<Collectable>().Collect();
                break;
        }
    }

    void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "Gate":
                other.GetComponent<Gate>().PassGate(PlayerTransform.position);
                break;
        }
    }

    #region HealthSystem
    public void TakeDamage(float damageTaken)
    {
        healthSystem.TakeDamage(damageTaken);
        Spawner.Instance.WorldTextPopup(((int)damageTaken).ToString(), PlayerTransform.position, Color.red);
    }

    private void OnDead()
    {
        GameManager.Instance.Reload();
    }
    #endregion

    #region LevelSystem
    public void AddExperience(int _amount)
    {
        PlayerData.Instance.exp += _amount;
        if (PlayerData.Instance.exp >= PlayerData.Instance.expPerLevel[PlayerData.Instance.level])
        {
            LevelUp();
        }
        expAmount = (float)PlayerData.Instance.exp / (float)PlayerData.Instance.expPerLevel[PlayerData.Instance.level];
        DOTween.To(() => slider_Exp.value, x => slider_Exp.value = x, expAmount, 0.25f).SetEase(Ease.Linear);
        PlayerPrefs.SetInt("Exp", PlayerData.Instance.exp);
    }

    private void LevelUp()
    {
        PlayerData.Instance.exp = 0;
        slider_Exp.value = 0;
        if (PlayerData.Instance.level < PlayerData.Instance.expPerLevel.Count - 1)
        {
            PlayerData.Instance.level += 1;
            txt_Level.text = "Lv." + (PlayerData.Instance.level + 1).ToString();
        }
        else
        {
            txt_Level.text = "Max";
        }

        particle_LevelUp.SetActive(true);
        PlayerPrefs.SetInt("Level", PlayerData.Instance.level);
        UIManager.Instance.EnablePanelUpgrade(true);
    }

    #endregion

    #region Collect


    #endregion
}
