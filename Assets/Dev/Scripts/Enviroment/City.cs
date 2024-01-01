using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    [SerializeField] private List<GameObject> ProbSets = new List<GameObject>();

    private int currentIndex = 0;

    public void RandomProps()
    {
        ProbSets[currentIndex].SetActive(false);
        currentIndex = Random.Range(0, ProbSets.Count);
        ProbSets[currentIndex].SetActive(true);
    }
}
