using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public Side side;
    private Enviroment environment;

    private void Awake()
    {
        environment = FindObjectOfType<Enviroment>();
    }

    public void PassGate(Vector3 exitPoint)
    {
        switch (side)
        {
            case Side.right:
                if (exitPoint.x > transform.position.x)
                    environment.MoveRight();
                break;
            case Side.left:
                if (exitPoint.x < transform.position.x)
                    environment.MoveLeft();
                break;
            case Side.up:
                if (exitPoint.z > transform.position.z)
                    environment.MoveUp();
                break;
            case Side.down:
                if (exitPoint.z < transform.position.z)
                    environment.MoveDown();
                break;
        }
    }
}
