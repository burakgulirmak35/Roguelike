using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    [SerializeField] private List<GameObject> ProbSets = new List<GameObject>();
    private List<SafeLanding> SafeLandings = new List<SafeLanding>();

    private int currentIndex = 0;

    void Awake()
    {
        for (int i = 0; i < ProbSets.Count; i++)
        {
            SafeLandings.Add(ProbSets[i].GetComponent<SafeLanding>());
        }
    }

    public Vector3 FindClosestSafePoint()
    {
        return SafeLandings[currentIndex].FindClosestSafePoint();
    }

    public void RandomProps()
    {
        ProbSets[currentIndex].SetActive(false);
        currentIndex = Random.Range(0, ProbSets.Count);
        ProbSets[currentIndex].SetActive(true);
    }
}
