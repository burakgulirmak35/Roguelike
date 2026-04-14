using System.Collections.Generic;
using UnityEngine;

public class SafeLanding : MonoBehaviour
{
    [SerializeField] List<Transform> SafePoints = new List<Transform>();

    public Vector3 FindClosestSafePoint()
    {
        Vector3 playerPos = Player.Instance.PlayerTransform.position;
        Transform closest = null;
        float minSqr = float.MaxValue;

        for (int i = 0; i < SafePoints.Count; i++)
        {
            float sqr = (SafePoints[i].position - playerPos).sqrMagnitude;
            if (sqr < minSqr)
            {
                minSqr = sqr;
                closest = SafePoints[i];
            }
        }

        return closest != null ? closest.position : playerPos;
    }
}
