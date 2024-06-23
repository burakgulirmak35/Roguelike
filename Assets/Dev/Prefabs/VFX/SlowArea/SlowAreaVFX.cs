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
        for (int i = 0; i < Spawner.Instance.ActiveEnemies.Count; i++)
        {
            Spawner.Instance.ActiveEnemies[i].GetComponent<Enemy>().SlowDown(slowAreaSO.SlowAreaPercent, slowAreaSO.SlowAreaTime);
        }
    }
}
