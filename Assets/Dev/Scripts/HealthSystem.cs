using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

[System.Serializable]
public class HealthSystem
{
    [SerializeField] private GameObject HealthBar;
    [SerializeField] private Slider HealthSlider;
    [SerializeField] private TextMeshProUGUI HealthTxt;
    private float maxHealth;
    private float health;
    private float healthAmount;

    [HideInInspector] public bool isAlive;
    public event Action OnDead;

    public void SetHealth(float _health)
    {
        maxHealth = _health;
        health = _health;
        healthAmount = 1f;
        HealthSlider.value = healthAmount;
        HealthTxt.text = ((int)health).ToString();
        isAlive = true;
    }

    public void Heal(float amount)
    {
        health += amount;
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
        healthAmount = health / maxHealth;
        DOTween.To(() => HealthSlider.value, x => HealthSlider.value = x, healthAmount, 0.25f).SetEase(Ease.Linear);
    }

    public void TakeDamage(float amount)
    {
        if (!isAlive) return;
        health -= amount;
        if (health <= 0)
        {
            health = 0;
            HealthSlider.value = 0;
            HealthTxt.text = ((int)health).ToString();
            OnDead?.Invoke();
            return;
        }
        healthAmount = health / maxHealth;
        DOTween.To(() => HealthSlider.value, x => HealthSlider.value = x, healthAmount, 0.2f).SetEase(Ease.Linear);
        HealthTxt.text = ((int)health).ToString();
    }
}
