using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshManager : MonoBehaviour
{
    public NavMeshSurface surfaces;
    private Transform playerTransform;

    public static NavMeshManager Instance { get; private set; }
    void Awake()
    {
        Instance = this;
        playerTransform = Player.Instance.PlayerTransform;
    }

    public void UpdateNavMesh()
    {
        surfaces.BuildNavMesh();
    }
}
