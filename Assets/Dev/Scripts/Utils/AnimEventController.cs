using System;
using UnityEngine;

public class AnimEventController : MonoBehaviour
{
    public event Action OnAttack;

    public void Attack()
    {
        OnAttack?.Invoke();
    }
}
