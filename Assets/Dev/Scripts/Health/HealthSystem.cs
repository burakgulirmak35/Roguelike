using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

[System.Serializable]
public class HealthSystem
{
    [SerializeField] private GameObject HealthBar;
    [SerializeField] private Slider slider_Health;
    [SerializeField] private TextMeshProUGUI txt_Health;
    [SerializeField] private ParticleSystem particle_SmallHeal;
    [SerializeField] private ParticleSystem particle_MediumHeal;
    [SerializeField] private ParticleSystem particle_LargeHeal;

    private float maxHealth;
    private float health;
    private float healthAmount;
    private float upgradeAmount;

    [HideInInspector] public bool isAlive;
    public event Action OnDead;

    public void SetHealth(float _health)
    {
        maxHealth = _health;
        health = _health;
        healthAmount = 1f;
        slider_Health.value = healthAmount;
        txt_Health.text = ((int)health).ToString();
        isAlive = true;
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

        health += amount;
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
        healthAmount = health / maxHealth;
        DOTween.To(() => slider_Health.value, x => slider_Health.value = x, healthAmount, 0.2f).SetEase(Ease.Linear);
    }

    public void HealPercent(float percent)
    {
        Heal(maxHealth * percent);
    }

    public void TakeDamage(float amount)
    {
        if (!isAlive) return;
        health -= amount;
        if (health <= 0)
        {
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
