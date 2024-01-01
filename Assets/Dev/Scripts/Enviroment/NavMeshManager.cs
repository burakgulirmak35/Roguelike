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


    private Vector3 BestPos;
    private float angle;
    private Vector3 CheckDir;
    public Vector3 NearestValidPosition()
    {
        NavMeshHit myNavHit;
        BestPos = playerTransform.position;
        BestPos.y = 1;
        if (NavMesh.SamplePosition(BestPos, out myNavHit, 10, NavMesh.GetAreaFromName("Walkable")))
        {
            return myNavHit.position;
        }

        for (int i = 0; i < 36; i++)
        {
            angle = 10f * i;
            CheckDir.x = Mathf.Sin(angle);
            CheckDir.z = Mathf.Cos(angle);
            CheckDir.y = 0;
            BestPos = playerTransform.position + CheckDir * 3;
            if (NavMesh.SamplePosition(BestPos, out myNavHit, 10, NavMesh.GetAreaFromName("Walkable")))
            {
                return myNavHit.position;
            }
        }
        return Vector3.zero;
    }
}
