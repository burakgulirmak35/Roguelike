using System;
using UnityEngine;

public class MyEnums : MonoBehaviour
{

}

[Serializable]
public enum PoolTypes
{
    Enemy,
    Bullet,
    BloodShot,
    WorldTextPopup,
    Collectable,
    BulletExplosion,
    SimpleExplosion,
}

[Serializable]
public enum ItemType
{
    Experience,
    Bomb,
    Booster,
    Health,
    HowerBoard,
    Magnet,
    SlowMotion,
    SpeedBoost,
}
