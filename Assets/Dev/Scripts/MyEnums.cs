using System;
using UnityEngine;

public class MyEnums : MonoBehaviour
{

}

[Serializable]
public enum PlayerState
{
    Normal,
    Stunned,
    Flying,
    Dead,
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
    MegaExplosion,
    Magnet,
    BloodLake,
}

[Serializable]
public enum ItemType
{
    Experience,
    Bomb,
    FireRateBoost,
    Health,
    MeshTrain,
    Magnet,
    SlowMotion,
    SpeedBoost,
}
