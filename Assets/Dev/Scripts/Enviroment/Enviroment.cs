
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public enum Side { left, right, up, down };
public class Enviroment : MonoBehaviour
{
    [Header("En Az 5 Parca")]
    [SerializeField] private List<GameObject> CityParts = new List<GameObject>();
    [Space]
    private GameObject PartMiddle;
    private GameObject PartLeft;
    private GameObject PartRight;
    private GameObject PartUp;
    private GameObject PartDown;
    [Space]
    [SerializeField] private Transform Gates;
    [Space]
    private int index;

    public static Enviroment Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    public void CreateCity()
    {
        index = Random.Range(0, CityParts.Count);
        PartMiddle = CityParts[index];
        CityParts.RemoveAt(index);

        index = Random.Range(0, CityParts.Count);
        PartLeft = CityParts[index];
        CityParts.RemoveAt(index);

        index = Random.Range(0, CityParts.Count);
        PartRight = CityParts[index];
        CityParts.RemoveAt(index);

        index = Random.Range(0, CityParts.Count);
        PartUp = CityParts[index];
        CityParts.RemoveAt(index);

        index = Random.Range(0, CityParts.Count);
        PartDown = CityParts[index];
        CityParts.RemoveAt(index);

        PartMiddle.transform.position = new Vector3(0, 0, 0);
        PartMiddle.transform.eulerAngles = RandomRotation();
        PartLeft.transform.eulerAngles = RandomRotation();
        PartRight.transform.eulerAngles = RandomRotation();
        PartUp.transform.eulerAngles = RandomRotation();
        PartDown.transform.eulerAngles = RandomRotation();

        AdjustSides();

        PartMiddle.SetActive(true);
        PartLeft.SetActive(true);
        PartRight.SetActive(true);
        PartUp.SetActive(true);
        PartDown.SetActive(true);

        for (int i = 0; i < CityParts.Count; i++)
        {
            CityParts[i].SetActive(false);
        }

        NavMeshManager.Instance.UpdateNavMesh();
        Gates.transform.position = PartMiddle.transform.position;
    }

    private void AdjustSides()
    {
        PartLeft.transform.position = PartMiddle.transform.position + new Vector3(-80, 0, 0);
        PartRight.transform.position = PartMiddle.transform.position + new Vector3(80, 0, 0);
        PartUp.transform.position = PartMiddle.transform.position + new Vector3(0, 0, 80);
        PartDown.transform.position = PartMiddle.transform.position + new Vector3(0, 0, -80);

        Spawner.Instance.SpawnPointsParent.transform.position = PartMiddle.transform.position;
    }

    private Vector3 RandomRotation()
    {
        switch (Random.Range(0, 4))
        {
            case 0:
                return new Vector3(0, 0, 0);
            case 1:
                return new Vector3(0, 90, 0);
            case 2:
                return new Vector3(0, 180, 0);
            case 3:
                return new Vector3(0, 270, 0);
            default:
                return new Vector3(0, 0, 0);
        }
    }

    public void MoveRight()
    {
        PartLeft.SetActive(false);
        CityParts.Add(PartLeft);

        PartLeft = PartMiddle;
        PartMiddle = PartRight;

        index = Random.Range(0, CityParts.Count);
        PartRight = CityParts[index];
        PartRight.transform.eulerAngles = RandomRotation();
        CityParts.RemoveAt(index);

        AdjustSides();
        PartRight.SetActive(true);

        Gates.transform.position = PartMiddle.transform.position;
        NavMeshManager.Instance.UpdateNavMesh();
    }

    public void MoveLeft()
    {
        PartRight.SetActive(false);
        CityParts.Add(PartRight);

        PartRight = PartMiddle;
        PartMiddle = PartLeft;

        index = Random.Range(0, CityParts.Count);
        PartLeft = CityParts[index];
        PartLeft.transform.eulerAngles = RandomRotation();
        CityParts.RemoveAt(index);

        AdjustSides();
        PartLeft.SetActive(true);

        Gates.transform.position = PartMiddle.transform.position;

        NavMeshManager.Instance.UpdateNavMesh();
    }

    public void MoveUp()
    {
        PartDown.SetActive(false);
        CityParts.Add(PartDown);

        PartDown = PartMiddle;
        PartMiddle = PartUp;

        index = Random.Range(0, CityParts.Count);
        PartUp = CityParts[index];
        PartUp.transform.eulerAngles = RandomRotation();
        CityParts.RemoveAt(index);

        AdjustSides();
        PartUp.SetActive(true);

        Gates.transform.position = PartMiddle.transform.position;

        NavMeshManager.Instance.UpdateNavMesh();
    }

    public void MoveDown()
    {
        PartUp.SetActive(false);
        CityParts.Add(PartUp);

        PartUp = PartMiddle;
        PartMiddle = PartDown;

        index = Random.Range(0, CityParts.Count);
        PartDown = CityParts[index];
        PartDown.transform.eulerAngles = RandomRotation();
        CityParts.RemoveAt(index);

        AdjustSides();
        PartDown.SetActive(true);

        Gates.transform.position = PartMiddle.transform.position;

        NavMeshManager.Instance.UpdateNavMesh();
    }
}
