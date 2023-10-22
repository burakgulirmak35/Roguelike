using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void TakeDamage(float damageTaken);
}

public interface ITargetable
{
    void Targeted(bool state);
}