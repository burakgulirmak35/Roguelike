using UnityEngine;

[CreateAssetMenu(menuName = "Roguelike/EnemySO")]
public class EnemySO : ScriptableObject
{
    public float Health;
    public float Speed;

    public float StartAttackRange;
    public float AttackRange;
    public float Damage;

    public int ExperienceAmount;
}