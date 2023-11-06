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
    [Header("Data")]
    private PlayerData playerData;

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
    [SerializeField] private ParticleSystem particle_LevelUp;
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

    public static Player Instance { get; private set; }
    private void Awake()
    {
        Instance = this;

        playerData = FindObjectOfType<PlayerData>();
        joystick = FindObjectOfType<FloatingJoystick>();
        Agent = GetComponent<NavMeshAgent>();
        Agent.updateRotation = false;
    }

    public void StartGame()
    {
        playerData.LoadData();
        EquipGun(playerData.SelectedGun);
        healthSystem.SetHealth(playerData.MaxHealth);

        StartCoroutine(TakeAimLoop());
        AddExperience(0);
    }

    public void CheckUpgrades()
    {
        // EquipGun(playerData.SelectedGun);
        healthSystem.UpgradeMaxHealth(playerData.MaxHealth);
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
        direction = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        Agent.Move(direction * playerData.MovementSpeed * Time.deltaTime);

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

            if (DistanceToEnemy <= playerData.FireRange)
            {
                EnemyInRange = true;
                PlayerAnim.SetBool("Aim", true);
                AimPoint.transform.position = new Vector3(TargetPoint.x, DefaultAimPoint.position.y, TargetPoint.z);
                Body.DOLookAt(new Vector3(TargetPoint.x, Body.position.y, TargetPoint.z), 0.1f);

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
                AimPoint.transform.position = DefaultAimPoint.position;

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
            AimPoint.transform.position = DefaultAimPoint.position;

            LeftArmIK.weight = 0;
            RightArmIK.weight = 0;
            BodyIK.weight = 0;
        }
    }
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
                other.GetComponent<Gate>().PassGate();
                break;
        }
    }

    #region HealthSystem
    public void TakeDamage(float damageTaken)
    {
        healthSystem.TakeDamage(damageTaken);
        Spawner.Instance.WorldTextPopup(((int)damageTaken).ToString(), transform.position, Color.red);
    }
    #endregion

    #region LevelSystem
    public void AddExperience(int _amount)
    {
        playerData.exp += _amount;
        if (playerData.exp >= playerData.expPerLevel[playerData.level])
        {
            OnLevelUp();
        }
        expAmount = (float)playerData.exp / (float)playerData.expPerLevel[playerData.level];
        DOTween.To(() => slider_Exp.value, x => slider_Exp.value = x, expAmount, 0.25f).SetEase(Ease.Linear);
        PlayerPrefs.SetInt("Exp", playerData.exp);
    }

    private void OnLevelUp()
    {
        playerData.exp = 0;
        slider_Exp.value = 0;
        if (playerData.expPerLevel.Count > playerData.level)
        {
            playerData.level += 1;
            txt_Level.text = "Lv." + (playerData.level + 1).ToString();
        }
        else
        {
            txt_Level.text = "Max";
        }

        particle_LevelUp.Play();
        PlayerPrefs.SetInt("Level", playerData.level);
        UIManager.Instance.EnablePanelUpgrade(true);
    }

    #endregion

    #region Collect


    #endregion
}
