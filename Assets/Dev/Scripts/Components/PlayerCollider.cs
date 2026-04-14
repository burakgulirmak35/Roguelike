using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gate"))
        {
            if (other.TryGetComponent<Gate>(out var gate))
                gate.PassGate();
        }
        else if (other.CompareTag("Collectable"))
        {
            if (other.TryGetComponent<Collectable>(out var collectable))
                collectable.Collect();
        }
    }
}
