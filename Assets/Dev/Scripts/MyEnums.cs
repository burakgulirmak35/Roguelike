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
    BulletExplosion,
    BloodShot,
    WorldTextPopup,
    Collectable,
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
