using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshManager : MonoBehaviour
{
    public NavMeshSurface surfaces;
    private Vector3 BestPos;


    public static NavMeshManager Instance { get; private set; }
    void Awake()
    {
        Instance = this;
    }

    public void UpdateNavMesh()
    {
        surfaces.BuildNavMesh();
    }

    public Vector3 NearestValidPosition()
    {
        NavMeshHit myNavHit;
        BestPos = Player.Instance.PlayerTransform.position;
        BestPos.y = 0;
        if (NavMesh.SamplePosition(BestPos, out myNavHit, 20, -1))
        {
            return myNavHit.position;
        }
        return BestPos;
    }
}
