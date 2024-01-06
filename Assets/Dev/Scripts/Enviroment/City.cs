using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    [SerializeField] private List<GameObject> ProbSets = new List<GameObject>();
    [SerializeField] public List<Transform> SpawnPoints = new List<Transform>();
    private List<SafeLanding> SafeLandings = new List<SafeLanding>();

    private int currentIndex = 0;

    void Awake()
    {
        if (ProbSets.Count <= 0) return;
        for (int i = 0; i < ProbSets.Count; i++)
        {
            SafeLandings.Add(ProbSets[i].GetComponent<SafeLanding>());
        }
        RandomProps();
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
