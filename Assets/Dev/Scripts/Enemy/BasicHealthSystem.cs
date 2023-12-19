using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;
using System.Collections;

public class BasicHealthSystem : MonoBehaviour
{
    [SerializeField] public GameObject HealthBar;
    [SerializeField] private Slider slider_Health;
    [SerializeField] private TextMeshProUGUI txt_Health;

    private float maxHealth;
    private float health;
    private float healthAmount;
    public event Action OnDead;

    public void SetHealth(float _health)
    {
        maxHealth = _health;
        health = _health;
        healthAmount = 1f;
        slider_Health.value = healthAmount;
        txt_Health.text = ((int)health).ToString();
    }

    public void Heal(float amount)
    {
        health += amount;
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
        healthAmount = health / maxHealth;
        DOTween.To(() => slider_Health.value, x => slider_Health.value = x, healthAmount, 0.25f).SetEase(Ease.Linear);
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
        if (health <= 0) return;

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
