using UnityEngine;

[CreateAssetMenu(menuName = "Roguelike/GunSO")]
public class GunSO : ScriptableObject
{
    [Header("Stats")]
    public float Damage;
    public float FireRate;
    public float FireRange;
    public int BurstCount;
    public float EachBurstTime;
    public float BulletSpeed;
    public bool Penetrability;
    public bool Bounce;
    public GunType gunType;
}

[System.Serializable]
public enum GunType
{
    Rifle, Pistol, ShotGun, Sniper, Grenade, Minigun, Rocket
}