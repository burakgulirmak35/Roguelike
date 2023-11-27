using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

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
    private PlayerInputActions playerInputActions;
    private InputAction moveAction;
    private InputAction aimAction;
    private InputAction toggleAimAction;
    [Space]
    private NavMeshAgent Agent;
    private Vector3 moveDirection;
    private Vector3 aimDirection;
    private Vector2 leftJoystick;
    private Vector2 rightJoystick;

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
    [HideInInspector] public bool isAutoAim = false;
    private bool isAiming = false;

    [Space]
    [HideInInspector] public Transform PlayerTransform;

    public static Player Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        Agent = GetComponent<NavMeshAgent>();

        Agent.updateRotation = false;
        PlayerTransform = this.transform;

        healthSystem.OnDead += OnDead;

        playerInputActions = new PlayerInputActions();

        moveAction = playerInputActions.Player.Move;
        moveAction.Enable();

        aimAction = playerInputActions.Player.Aim;
        aimAction.Enable();

        toggleAimAction = playerInputActions.Player.ToggleAim;
        toggleAimAction.performed += ToggleAim;
        toggleAimAction.Enable();
    }

    public void StartGame()
    {
        PlayerData.Instance.LoadData();
        EquipGun(PlayerData.Instance.SelectedGun);
        healthSystem.SetHealth(PlayerData.Instance.MaxHealth);
        AddExperience(0);
        PrepareAim();
    }

    private void PrepareAim()
    {
        Aim(false);
        isAutoAim = PlayerPrefs.GetInt("AutoAim") == 1 ? true : false;
        UIManager.Instance.ToggleAim();
        if (isAutoAim)
        {
            AutoAimCoro = StartCoroutine(AutoAimLoop());
        }
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
    private float velocityZ;
    private float velocityX;
    private void Update()
    {
        JoystickMove();
        if (!isAutoAim)
        {
            JoystickAim();
        }
    }

    #region --- Movement ---
    private void JoystickMove()
    {
        leftJoystick = moveAction.ReadValue<Vector2>();
        moveDirection.x = leftJoystick.x;
        moveDirection.z = leftJoystick.y;

        Agent.Move(moveDirection * PlayerData.Instance.MovementSpeed * Time.deltaTime);
        if (moveDirection.magnitude > 0 && !isAiming)
        {
            Body.forward = moveDirection;
        }

        velocityZ = Vector3.Dot(moveDirection.normalized, Body.forward);
        velocityX = Vector3.Dot(moveDirection.normalized, Body.right);

        PlayerAnim.SetFloat("Speed", moveDirection.magnitude);
        PlayerAnim.SetFloat("Vertical", velocityZ);
        PlayerAnim.SetFloat("Horizontal", velocityX);
    }
    #endregion

    #region GunFire
    public void ToggleAim(InputAction.CallbackContext callbackContext)
    {
        Aim(false);
        isAutoAim = !isAutoAim;
        UIManager.Instance.ToggleAim();
        PlayerPrefs.SetInt("AutoAim", isAutoAim ? 1 : 0);

        if (isAutoAim)
        {
            if (AutoAimCoro != null)
            {
                StopCoroutine(AutoAimCoro);
            }
            AutoAimCoro = StartCoroutine(AutoAimLoop());
        }
        else
        {
            if (AutoAimCoro != null)
            {
                StopCoroutine(AutoAimCoro);
            }
        }
    }

    #region ---JoystickAim---
    private void Aim(bool state)
    {
        if (state)
        {
            isAiming = true;
            PlayerAnim.SetBool("Aim", true);
            LeftArmIK.weight = 1;
            RightArmIK.weight = 1;
            BodyIK.weight = 1;

            gun.StartFire();
        }
        else
        {
            gun.StopFire();

            isAiming = false;
            PlayerAnim.SetBool("Aim", false);
            AimPoint.position = DefaultAimPoint.position;
            LeftArmIK.weight = 0;
            RightArmIK.weight = 0;
            BodyIK.weight = 0;
        }

    }
    #endregion

    #region ---JoystickAim---
    private void JoystickAim()
    {
        rightJoystick = aimAction.ReadValue<Vector2>();
        aimDirection.x = rightJoystick.x;
        aimDirection.z = rightJoystick.y;
        aimDirection = aimDirection.normalized;

        if (rightJoystick.magnitude > 0)
        {
            TargetPoint = Body.position + aimDirection;
            Body.forward = aimDirection;

            TargetPoint.y = DefaultAimPoint.position.y;
            AimPoint.position = TargetPoint;

            if (!isAiming)
            {
                Aim(true);
            }
        }
        else if (isAiming)
        {
            Aim(false);
        }
    }
    #endregion

    #region ---AutoAim---
    private Coroutine AutoAimCoro;
    private IEnumerator AutoAimLoop()
    {
        while (Alive)
        {
            AutoAim();
            yield return new WaitForSeconds(0.1f);
        }
    }
    private void AutoAim()
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
            TargetPoint.y = DefaultAimPoint.position.y;

            if (DistanceToEnemy <= PlayerData.Instance.FireRange)
            {
                AimPoint.position = TargetPoint;
                TargetPoint.y = Body.position.y;
                Body.DOLookAt(TargetPoint, 0.1f);
                Aim(true);
            }
            else
            {
                AimPoint.position = DefaultAimPoint.position;
                Aim(false);
            }
        }
        else
        {
            AimPoint.position = DefaultAimPoint.position;
            Aim(false);
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

    private int RandomAnimationIndex()
    {
        return Random.Range(0, 4);
    }
    private void OnDead()
    {
        this.enabled = false;
        Agent.enabled = false;

        Aim(false);
        Alive = false;

        PlayerAnim.Play("Dead " + RandomAnimationIndex().ToString());
        UIManager.Instance.EnablePanelPlayerDead(true);
    }

    public void ReBorn()
    {
        Agent.enabled = true;
        PlayerAnim.SetBool("Alive", true);
        healthSystem.SetHealth(PlayerData.Instance.MaxHealth);
        Alive = true;

        this.enabled = true;
        PrepareAim();

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
