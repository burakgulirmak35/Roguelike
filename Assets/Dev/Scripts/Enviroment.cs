
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using Unity.VisualScripting;


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
    public NavMeshSurface[] surfaces;
    [SerializeField] private Transform Gates;
    [SerializeField] private Transform InverseGates;
    [Space]
    private int index;

    public static Enviroment Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        CreateCity();
    }

    private void UpdateNavMesh()
    {
        for (int i = 0; i < surfaces.Length; i++)
        {
            surfaces[i].BuildNavMesh();
        }
    }

    private void CreateCity()
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
        PartLeft.transform.position = new Vector3(-120, 0, 0);
        PartRight.transform.position = new Vector3(120, 0, 0);
        PartUp.transform.position = new Vector3(0, 0, 120);
        PartDown.transform.position = new Vector3(0, 0, -120);

        PartMiddle.SetActive(true);
        PartLeft.SetActive(true);
        PartRight.SetActive(true);
        PartUp.SetActive(true);
        PartDown.SetActive(true);

        for (int i = 0; i < CityParts.Count; i++)
        {
            CityParts[i].SetActive(false);
        }

        UpdateNavMesh();
        Gates.transform.position = PartMiddle.transform.position;
        InverseGates.transform.position = new Vector3(0, -10, 0);
    }

    private void AdjustSides()
    {
        PartLeft.transform.position = PartMiddle.transform.position + new Vector3(-120, 0, 0);
        PartRight.transform.position = PartMiddle.transform.position + new Vector3(120, 0, 0);
        PartUp.transform.position = PartMiddle.transform.position + new Vector3(0, 0, 120);
        PartDown.transform.position = PartMiddle.transform.position + new Vector3(0, 0, -120);
    }

    public void MoveRight()
    {
        PartLeft.SetActive(false);
        CityParts.Add(PartLeft);

        PartLeft = PartMiddle;
        PartMiddle = PartRight;

        index = Random.Range(0, CityParts.Count);
        PartRight = CityParts[index];
        CityParts.RemoveAt(index);

        AdjustSides();
        PartRight.SetActive(true);

        Gates.transform.position = PartMiddle.transform.position;
        InverseGates.transform.position = PartLeft.transform.position;

        UpdateNavMesh();
    }

    public void MoveLeft()
    {
        PartRight.SetActive(false);
        CityParts.Add(PartRight);

        PartRight = PartMiddle;
        PartMiddle = PartLeft;

        index = Random.Range(0, CityParts.Count);
        PartLeft = CityParts[index];
        CityParts.RemoveAt(index);

        AdjustSides();
        PartLeft.SetActive(true);

        Gates.transform.position = PartMiddle.transform.position;
        InverseGates.transform.position = PartLeft.transform.position;

        UpdateNavMesh();
    }

    public void MoveUp()
    {
        PartDown.SetActive(false);
        CityParts.Add(PartDown);

        PartDown = PartMiddle;
        PartMiddle = PartUp;

        index = Random.Range(0, CityParts.Count);
        PartUp = CityParts[index];
        CityParts.RemoveAt(index);

        AdjustSides();
        PartUp.SetActive(true);

        Gates.transform.position = PartMiddle.transform.position;
        InverseGates.transform.position = PartDown.transform.position;

        UpdateNavMesh();
    }

    public void MoveDown()
    {
        PartUp.SetActive(false);
        CityParts.Add(PartUp);

        PartUp = PartMiddle;
        PartMiddle = PartDown;

        index = Random.Range(0, CityParts.Count);
        PartDown = CityParts[index];
        CityParts.RemoveAt(index);

        AdjustSides();
        PartDown.SetActive(true);

        Gates.transform.position = PartMiddle.transform.position;
        InverseGates.transform.position = PartUp.transform.position;

        UpdateNavMesh();
    }
}
