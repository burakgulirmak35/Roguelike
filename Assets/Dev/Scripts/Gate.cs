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

    public void PassGate()
    {
        switch (side)
        {
            case Side.left:
                environment.MoveLeft();
                break;
            case Side.right:
                environment.MoveRight();
                break;
            case Side.up:
                environment.MoveUp();
                break;
            case Side.down:
                environment.MoveDown();
                break;
        }
    }
}
