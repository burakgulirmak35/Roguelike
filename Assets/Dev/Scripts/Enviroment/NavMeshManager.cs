using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshManager : MonoBehaviour
{
    public NavMeshSurface surfaces;
    public static NavMeshManager Instance { get; private set; }
    void Awake()
    {
        Instance = this;
    }

    public void UpdateNavMesh(float _time = 0)
    {
        if (_time > 0)
            StartCoroutine(UpdateNavMeshTimer(_time));
        else
            surfaces.BuildNavMesh();
    }

    private IEnumerator UpdateNavMeshTimer(float _time)
    {
        yield return new WaitForSeconds(_time);
        surfaces.BuildNavMesh();
    }
}
