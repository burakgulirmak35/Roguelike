using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    [SerializeField] private List<GameObject> ProbSets = new List<GameObject>();
    [SerializeField] public List<Transform> SpawnPoints = new List<Transform>();
    void Awake()
    {
        if (ProbSets.Count > 1) Open(1);
    }

    public void Open(int index)
    {
        for (int i = 0; i < ProbSets.Count; i++)
        {
            ProbSets[i].SetActive(i == index);
        }
    }
}
