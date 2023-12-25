using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Gate":

                break;
            case "Collectable":
                other.GetComponent<Collectable>().Collect();
                break;
        }
    }

    void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "Gate":
                other.GetComponent<Gate>().PassGate(Player.Instance.PlayerTransform.position);
                break;
        }
    }
}
