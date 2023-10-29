using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Apocalypse/EnemySO", order = 0)]
public class EnemySO : ScriptableObject
{

    public float Health;
    public float Speed;

    public float AttackRange;
    public float Damage;
}