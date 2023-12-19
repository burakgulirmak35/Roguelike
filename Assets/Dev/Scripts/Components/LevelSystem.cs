using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelSystem : MonoBehaviour
{
    [SerializeField] private Slider slider_Exp;
    [SerializeField] private TextMeshProUGUI txt_Level;
    [SerializeField] private GameObject particle_LevelUp;
    private float expAmount;

    public void SetLevel()
    {
        AddExperience(0);
    }

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
}
