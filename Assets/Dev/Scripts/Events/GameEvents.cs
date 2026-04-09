using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action<Transform, Vector3> OnEnemyKilled;
    public static event Action                     OnPlayerDead;
    public static event Action<int>                OnLevelUp;
    public static event Action<float, Vector3, bool> OnDamageTaken;

    public static event Action<float> OnHealRequest;

    public static void RaiseEnemyKilled(Transform t, Vector3 pos)                      => OnEnemyKilled?.Invoke(t, pos);
    public static void RaisePlayerDead()                                                => OnPlayerDead?.Invoke();
    public static void RaiseLevelUp(int level)                                          => OnLevelUp?.Invoke(level);
    public static void RaiseDamageTaken(float amount, Vector3 pos, bool isPlayer)      => OnDamageTaken?.Invoke(amount, pos, isPlayer);
    public static void RaiseHealRequest(float percent)                                  => OnHealRequest?.Invoke(percent);
}
