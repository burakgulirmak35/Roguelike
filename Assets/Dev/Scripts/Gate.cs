using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public Side side;
    private Enviroment enviroment;

    private void Awake()
    {
        enviroment = FindObjectOfType<Enviroment>();
    }

    public void PassGate()
    {
        switch (side)
        {
            case Side.left:
                enviroment.MoveLeft();
                break;
            case Side.right:
                enviroment.MoveRight();
                break;
            case Side.up:
                enviroment.MoveUp();
                break;
            case Side.down:
                enviroment.MoveDown();
                break;
        }
    }
}
