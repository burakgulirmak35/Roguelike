using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;
using System.Collections;

public class EnemyHealthSystem : MonoBehaviour
{
    [SerializeField] public GameObject HealthBar;
    [SerializeField] private Slider slider_Health;
    [SerializeField] private TextMeshProUGUI txt_Health;

    private float maxHealth;
    private float health;
    private float healthAmount;
    private int _lastDisplayedHealth = -1;
    private Tweener _healthTween;
    [HideInInspector] public bool isAlive;
    private Transform myTransform;

    public event Action OnDead;

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
        _lastDisplayedHealth = (int)health;
        txt_Health.text = _lastDisplayedHealth.ToString();
    }

    public void Heal(float amount)
    {
        health += amount;
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
        healthAmount = health / maxHealth;
        _healthTween?.Kill();
        _healthTween = DOTween.To(() => slider_Health.value, x => slider_Health.value = x, healthAmount, 0.25f).SetEase(Ease.Linear);
        UpdateHealthText();
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
        if (!isAlive) return;

        ShowHealth();
        GameEvents.RaiseDamageTaken(amount, myTransform.position, isPlayer: false);

        health -= amount;
        if (health <= 0)
        {
            isAlive = false;
            health = 0;
            _healthTween?.Kill();
            slider_Health.value = 0;
            txt_Health.text = "0";
            _lastDisplayedHealth = 0;
            OnDead?.Invoke();
            return;
        }
        healthAmount = health / maxHealth;
        _healthTween?.Kill();
        _healthTween = DOTween.To(() => slider_Health.value, x => slider_Health.value = x, healthAmount, 0.2f).SetEase(Ease.Linear);
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        int displayVal = (int)health;
        if (displayVal != _lastDisplayedHealth)
        {
            _lastDisplayedHealth = displayVal;
            txt_Health.text = displayVal.ToString();
        }
    }

    #region HeathVisualTimer
    private Coroutine ShowHealthCoro;
    private IEnumerator ShowHealthTimer()
    {
        HealthBar.SetActive(true);
        yield return new WaitForSeconds(10f);
        HealthBar.SetActive(false);
    }
    public void HideHealth()
    {
        if (ShowHealthCoro != null)
        {
            StopCoroutine(ShowHealthCoro);
            ShowHealthCoro = null;
            HealthBar.SetActive(false);
        }
    }

    public void ShowHealth()
    {
        if (ShowHealthCoro != null)
        {
            StopCoroutine(ShowHealthCoro);
            ShowHealthCoro = null;
        }

        if (health > 0)
        {
            ShowHealthCoro = StartCoroutine(ShowHealthTimer());
        }
    }
    #endregion
}
