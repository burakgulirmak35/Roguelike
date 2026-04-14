using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    [SerializeField] private List<GameObject> ProbSets = new List<GameObject>();
    [SerializeField] public List<Transform> SpawnPoints = new List<Transform>();
    private SafeLanding activeSafeLanding;

    void Awake()
    {
        if (ProbSets.Count > 1) Open(1);
    }

    public Vector3 FindClosestSafePoint()
    {
        if (activeSafeLanding == null) return transform.position;
        return activeSafeLanding.FindClosestSafePoint();
    }

    public void Open(int index)
    {
        for (int i = 0; i < ProbSets.Count; i++)
        {
            ProbSets[i].SetActive(i == index);
        }
        activeSafeLanding = ProbSets[index].GetComponent<SafeLanding>();
    }
}
