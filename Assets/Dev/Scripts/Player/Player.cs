using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [Header("---Components---")]
    [SerializeField] public HealthSystem healthSystem;
    [SerializeField] public LevelSystem levelSystem;
    [SerializeField] public EffectSystem effectSystem;
    [SerializeField] public CostumeSystem costumeSystem;
    [SerializeField] public PlayerCollider playerCollider;

    [Header("Gun")]
    private Gun gun;

    [Header("Vehicle")]
    [SerializeField] public HoverBoard hoverBoard;

    [Header("Animations")]
    [SerializeField] private Animator PlayerAnim;

    [Header("Model")]
    [SerializeField] public Transform Body;
    [SerializeField] public Transform Holder;
    [SerializeField] private GameObject Rifle;
    [Space]
    private PlayerInputActions playerInputActions;
    private InputAction moveAction;
    private InputAction aimAction;
    private InputAction toggleAimAction;
    private InputAction mapAction;
    [Space]
    [HideInInspector] public NavMeshAgent PlayerAgent;
    private Vector3 moveDirection;
    private Vector3 aimDirection;
    private Vector2 leftJoystick;
    private Vector2 rightJoystick;

    [Header("Aim")]
    [SerializeField] private Transform AimPoint;
    [SerializeField] private Transform DefaultAimPoint;
    [SerializeField] private Transform LeftArmTarget;
    [Space]
    [SerializeField] private TwoBoneIKConstraint LeftArmIK;
    [SerializeField] private MultiAimConstraint RightArmIK;
    [SerializeField] private MultiAimConstraint BodyIK;
    [Space]
    [SerializeField] public TwoBoneIKConstraint LeftLegIK;
    [SerializeField] public TwoBoneIKConstraint RightLegIK;
    [Space]
    private Vector3 TargetPoint;
    private Transform ClosestEnemy;

    [Header("State")]
    [HideInInspector] public bool isAutoAim = false;
    private bool isAiming = false;

    [Space]
    [HideInInspector] public Transform PlayerTransform;
    private ITargetable _closestTargetable;
    private float _fireRangeSqr;


    public static Player Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        PlayerAgent = GetComponent<NavMeshAgent>();
        PlayerAgent.updateRotation = false;
        PlayerTransform = this.transform;
        playerState = PlayerState.Normal;

        healthSystem.OnDead += OnDead;
    }

    public void SetInputs()
    {
        playerInputActions = new PlayerInputActions();
        #region Move
        moveAction = playerInputActions.Player.Move;
        moveAction.Enable();
        #endregion

        #region Aim
        aimAction = playerInputActions.Player.Aim;
        aimAction.Enable();
        #endregion

        #region ToggleAim
        toggleAimAction = playerInputActions.Player.ToggleAim;
        toggleAimAction.performed += ToggleAim;
        toggleAimAction.Enable();
        #endregion

        #region ClickMap
        mapAction = playerInputActions.Player.ClickMap;
        mapAction.performed += ClickMap;
        mapAction.Enable();
        #endregion
    }

    public void ClearInputs()
    {
        toggleAimAction.performed -= ToggleAim;
        mapAction.performed -= ClickMap;

        playerInputActions.Player.Disable();
        playerInputActions.Dispose();
    }

    #region --- Map ---
    private void ClickMap(InputAction.CallbackContext callbackContext)
    {
        CameraManager.Instance.CamChangePos();
    }

    #endregion

    public void StartGame()
    {
        SetInputs();
        PlayerData.Instance.LoadData();
        EquipGun(PlayerData.Instance.SelectedGun);

        costumeSystem.SetCharacter();
        healthSystem.SetHealth(PlayerData.Instance.MaxHealth);
        levelSystem.SetLevel();
        effectSystem.SetEffects();
        PrepareAim();

        CameraManager.Instance.CamDefaultPos();
    }

    private void PrepareAim()
    {
        Aim(false);
        isAutoAim = PlayerPrefs.GetInt("AutoAim", 1) == 1 ? true : false;
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
    private event UnityAction Movement;
    private void JoystickMove()
    {
        leftJoystick = moveAction.ReadValue<Vector2>();

        moveDirection.x = leftJoystick.x;
        moveDirection.z = leftJoystick.y;
        velocityZ = Vector3.Dot(moveDirection.normalized, Body.forward);
        velocityX = Vector3.Dot(moveDirection.normalized, Body.right);

        if (moveDirection.magnitude > 0 && !isAiming)
            Body.forward = moveDirection;

        Movement?.Invoke();
    }

    private void DefaultMovement()
    {
        PlayerAgent.Move(moveDirection * PlayerData.Instance.MovementSpeed * PlayerData.Instance.MovementSpeedMultipler * Time.deltaTime);

        PlayerAnim.SetFloat("Speed", moveDirection.magnitude);
        PlayerAnim.SetFloat("Vertical", velocityZ);
        PlayerAnim.SetFloat("Horizontal", velocityX);
    }

    private void HoverMovement()
    {
        PlayerAgent.Move(moveDirection * PlayerData.Instance.MovementSpeed * PlayerData.Instance.MovementSpeedMultipler * Time.deltaTime);
        hoverBoard.BoardTransform.forward = moveDirection;

        PlayerAnim.SetFloat("Speed", moveDirection.magnitude);
        PlayerAnim.SetFloat("Vertical", 0);
        PlayerAnim.SetFloat("Horizontal", 0);
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
            UnTarget();
            if (AutoAimCoro != null)
            {
                StopCoroutine(AutoAimCoro);
            }
        }
    }
    #endregion

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
    private static readonly WaitForSeconds _waitAutoAim = new WaitForSeconds(0.1f);
    private Coroutine AutoAimCoro;
    private IEnumerator AutoAimLoop()
    {
        while (healthSystem.isAlive)
        {
            AutoAim();
            yield return _waitAutoAim;
        }
    }

    private void AutoAim()
    {
        UnTarget();

        var enemies = EnemyManager.Instance.GetEnemies();
        Transform closestTransform = null;
        float minSqr = float.MaxValue;
        Vector3 myPos = PlayerTransform.position;

        for (int i = 0; i < enemies.Count; i++)
        {
            var e = enemies[i];
            if (!e.enemyHealthSystem.isAlive) continue;
            float sqr = (e.transform.position - myPos).sqrMagnitude;
            if (sqr < minSqr)
            {
                minSqr = sqr;
                closestTransform = e.transform;
                _closestTargetable = e;
            }
        }

        if (closestTransform == null)
        {
            AimPoint.position = DefaultAimPoint.position;
            Aim(false);
            return;
        }

        ClosestEnemy = closestTransform;
        _closestTargetable.Targeted(true);

        _fireRangeSqr = PlayerData.Instance.FireRange * PlayerData.Instance.FireRange;
        if (minSqr <= _fireRangeSqr)
        {
            TargetPoint = ClosestEnemy.position;
            TargetPoint.y = 1.16f;
            AimPoint.DOMove(TargetPoint, 0.1f);

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

    private void UnTarget()
    {
        if (_closestTargetable != null)
        {
            _closestTargetable.Targeted(false);
            _closestTargetable = null;
        }
    }
    #endregion



    #region DeadAlive
    private static readonly string[] DeadAnims = { "Dead 0", "Dead 1", "Dead 2", "Dead 3" };

    private void OnDead()
    {
        playerState = PlayerState.Dead;

        Aim(false);
        PlayerAnim.Play(DeadAnims[Random.Range(0, DeadAnims.Length)]);
        GameEvents.RaisePlayerDead();
    }
    public void ReBorn()
    {
        playerState = PlayerState.Normal;

        Spawner.Instance.SpawnAtPos(PoolTypes.MegaExplosion, effectSystem.EffectTransform.position);
        PlayerAnim.SetBool("Alive", true);
        healthSystem.SetHealth(PlayerData.Instance.MaxHealth);
        healthSystem.isAlive = true;
        PrepareAim();
    }
    #endregion

    #region State
    private PlayerState _playerState;
    public PlayerState playerState
    {
        get
        {
            return _playerState;
        }
        set
        {
            _playerState = value;
            switch (_playerState)
            {
                case PlayerState.Dead:
                    PlayerAgent.enabled = false;
                    healthSystem.isAlive = false;
                    healthSystem.isDamageble = false;
                    Movement = null;
                    break;
                case PlayerState.Normal:
                    PlayerAgent.enabled = true;
                    healthSystem.isDamageble = true;
                    Movement = DefaultMovement;
                    break;
                case PlayerState.HoverBoard:
                    PlayerAgent.enabled = true;
                    healthSystem.isDamageble = false;
                    Movement = HoverMovement;
                    break;
                case PlayerState.Stunned:
                    PlayerAgent.enabled = true;
                    Movement = null;
                    break;
                case PlayerState.CutScene:
                    PlayerAgent.enabled = false;
                    Movement = null;
                    break;
            }
        }
    }
    #endregion
}
