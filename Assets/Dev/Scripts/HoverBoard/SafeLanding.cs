using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeLanding : MonoBehaviour
{
    private KdTree<Transform> Points = new KdTree<Transform>();
    [SerializeField] List<Transform> SafePoints = new List<Transform>();

    void Awake()
    {
        Points.AddAll(SafePoints);
    }

    public Vector3 FindClosestSafePoint()
    {
        return Points.FindClosest(Player.Instance.PlayerTransform.position).position;
    }
}
