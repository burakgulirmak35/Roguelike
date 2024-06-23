using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private Slider slider_Health;
    [SerializeField] private TextMeshProUGUI txt_Health;
    [SerializeField] private ParticleSystem particle_SmallHeal;
    [SerializeField] private ParticleSystem particle_MediumHeal;
    [SerializeField] private ParticleSystem particle_LargeHeal;
    [SerializeField] private Sound HealSound;

    private float maxHealth;
    private float health;
    private float healthAmount;
    private float upgradeAmount;

    [HideInInspector] public bool isDamageble;
    [HideInInspector] public bool isAlive;
    public event Action OnDead;

    private Transform myTransform;

    void Awake()
    {
        myTransform = transform;
    }

    public void SetHealth(float _health)
    {
        maxHealth = _health;
        health = _health;
        healthAmount = 1f;
        slider_Health.value = healthAmount;
        txt_Health.text = ((int)health).ToString();
        isAlive = true;
        isDamageble = true;
    }

    public void UpgradeMaxHealth(float newMaxHeath)
    {
        if (maxHealth < newMaxHeath)
        {
            upgradeAmount = newMaxHeath - maxHealth;
            maxHealth = newMaxHeath;
            Heal(upgradeAmount);
        }
    }

    public void Heal(float amount)
    {
        if (amount >= maxHealth * 0.75f) { particle_LargeHeal.Play(); }
        else if (amount >= maxHealth * 0.5f) { particle_MediumHeal.Play(); }
        else if (amount > 0) { particle_SmallHeal.Play(); }
        SoundManager.Instance.PlaySound(HealSound);

        health += amount;
        if (health >= maxHealth)
        {
            health = maxHealth;
        }

        healthAmount = health / maxHealth;
        DOTween.To(() => slider_Health.value, x => slider_Health.value = x, healthAmount, 0.2f).SetEase(Ease.Linear);
        txt_Health.text = ((int)health).ToString();
    }

    public void HealPercent(float percent)
    {
        Heal(maxHealth * percent);
    }

    public void FullHeal()
    {
        Heal(maxHealth);
    }

    public void TakeDamage(float amount)
    {
        if (!isDamageble) return;
        if (!isAlive) return;

        health -= amount;
        Spawner.Instance.WorldTextPopup(((int)amount).ToString(), myTransform.position, Color.red);

        if (health <= 0)
        {
            isAlive = false;
            health = 0;
            slider_Health.value = 0;
            txt_Health.text = ((int)health).ToString();
            OnDead?.Invoke();
            return;
        }
        healthAmount = health / maxHealth;
        DOTween.To(() => slider_Health.value, x => slider_Health.value = x, healthAmount, 0.2f).SetEase(Ease.Linear);
        txt_Health.text = ((int)health).ToString();
    }
}
