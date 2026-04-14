using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowAreaVFX : MonoBehaviour
{
    [SerializeField] private SlowAreaSO slowAreaSO;
    [SerializeField] private Sound sound;

    void OnEnable()
    {
        SoundManager.Instance.PlaySound(sound);
        var enemies = EnemyManager.Instance.GetEnemies();
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].SlowDown(slowAreaSO.SlowAreaPercent, slowAreaSO.SlowAreaTime);
        }
    }
}
