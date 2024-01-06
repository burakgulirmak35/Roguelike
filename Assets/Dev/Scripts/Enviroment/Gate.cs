using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private City EnteredCity;

    public void PassGate()
    {
        Enviroment.Instance.CurrentCity = EnteredCity;
    }
}
