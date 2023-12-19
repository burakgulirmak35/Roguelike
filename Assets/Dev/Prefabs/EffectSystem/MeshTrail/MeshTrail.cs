using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTrail : MonoBehaviour
{
    [SerializeField] private float meshRefreshRate;
    [SerializeField] private int maxGhostCount;
    [SerializeField] private Material material;
    [Space]
    [HideInInspector] public SkinnedMeshRenderer skinnedMeshRenderer;
    [Space]
    private List<GameObject> GhostObjects = new List<GameObject>();
    private List<Transform> GhostTransforms = new List<Transform>();
    private List<MeshFilter> mf = new List<MeshFilter>();
    private List<Mesh> mesh = new List<Mesh>();
    private GameObject go;
    private int ghostIndex;

    private void Awake()
    {
        for (int i = 0; i < maxGhostCount; i++)
        {
            go = new GameObject();
            go.AddComponent<MeshRenderer>().material = material;

            GhostObjects.Add(go);
            GhostTransforms.Add(go.transform);
            mf.Add(go.AddComponent<MeshFilter>());
            mesh.Add(new Mesh());
            go.SetActive(false);
        }
    }

    private bool isTrailActive;
    public void ActiveTrail()
    {
        if (!isTrailActive)
        {
            isTrailActive = true;
            StartCoroutine(ActivateTrailTimer());
        }
    }

    private IEnumerator ActivateTrailTimer()
    {
        for (int i = 0; i < maxGhostCount; i++)
        {
            skinnedMeshRenderer.BakeMesh(mesh[ghostIndex]);
            mf[ghostIndex].mesh = mesh[ghostIndex];

            GhostTransforms[ghostIndex].position = Player.Instance.Body.position;
            GhostTransforms[ghostIndex].rotation = Player.Instance.Body.rotation;

            GhostObjects[ghostIndex].SetActive(true);

            ghostIndex++;
            if (ghostIndex >= GhostObjects.Count)
                ghostIndex = 0;

            yield return new WaitForSeconds(meshRefreshRate);
        }
        StartCoroutine(DisableTrailTimer());
    }

    private IEnumerator DisableTrailTimer()
    {
        for (int i = 0; i < maxGhostCount; i++)
        {
            yield return new WaitForSeconds(meshRefreshRate);
            GhostObjects[i].SetActive(false);
            Spawner.Instance.SpawnAtPos(PoolTypes.SimpleExplosion, GhostTransforms[i].position + Vector3.up * 2);
        }
        isTrailActive = false;
    }
}
