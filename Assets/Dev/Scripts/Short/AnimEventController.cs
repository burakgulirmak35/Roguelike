using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventController : MonoBehaviour
{
    public event Action OnAttack;
    public event Action OnAttackEnd;

    public void Attack()
    {
        OnAttack?.Invoke();
    }

    public void AttackEnd()
    {
        OnAttackEnd?.Invoke();
    }

}
