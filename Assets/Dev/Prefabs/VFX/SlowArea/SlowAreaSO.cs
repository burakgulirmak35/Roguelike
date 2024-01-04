using UnityEngine;

[CreateAssetMenu(fileName = "SlowAreaSO", menuName = "Roguelike/SlowAreaSO")]
public class SlowAreaSO : ScriptableObject
{
    public float SlowAreaTime;
    public float SlowAreaPercent;
    public float SlowAreaDistance;
}