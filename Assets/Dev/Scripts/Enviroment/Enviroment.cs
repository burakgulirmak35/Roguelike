
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enviroment : MonoBehaviour
{
    [SerializeField] public City CurrentCity;
    public static Enviroment Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
