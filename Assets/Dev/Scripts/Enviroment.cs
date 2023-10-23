
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
    public NavMeshSurface[] surfaces;
    [SerializeField] private Transform Gates;
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
            // surfaces[i].BuildNavMesh();
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
    }

    private void ReplaceSide(Side _side)
    {
        index = Random.Range(0, CityParts.Count);
        switch (_side)
        {
            case Side.left:
                PartLeft.SetActive(false);
                CityParts.Add(PartLeft);
                PartLeft = CityParts[index];
                PartLeft.transform.position = PartMiddle.transform.position + new Vector3(-120, 0, 0);
                break;
            case Side.right:
                PartRight.SetActive(false);
                CityParts.Add(PartRight);
                PartRight = CityParts[index];
                PartRight.transform.position = PartMiddle.transform.position + new Vector3(120, 0, 0);
                break;
            case Side.up:
                PartUp.SetActive(false);
                CityParts.Add(PartUp);
                PartUp = CityParts[index];
                PartUp.transform.position = PartMiddle.transform.position + new Vector3(0, 0, 120);
                break;
            case Side.down:
                PartDown.SetActive(false);
                CityParts.Add(PartDown);
                PartDown = CityParts[index];
                PartDown.transform.position = PartMiddle.transform.position + new Vector3(0, 0, -120);
                break;
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
        CityParts.RemoveAt(index);

        PartRight.transform.position = PartMiddle.transform.position + new Vector3(120, 0, 0);
        PartRight.SetActive(true);

        ReplaceSide(Side.up);
        ReplaceSide(Side.down);

        Gates.transform.position = PartMiddle.transform.position;
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

        PartLeft.transform.position = PartMiddle.transform.position + new Vector3(-120, 0, 0);
        PartLeft.SetActive(true);

        ReplaceSide(Side.up);
        ReplaceSide(Side.down);

        Gates.transform.position = PartMiddle.transform.position;
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

        PartUp.transform.position = PartMiddle.transform.position + new Vector3(0, 0, 120);
        PartUp.SetActive(true);

        ReplaceSide(Side.left);
        ReplaceSide(Side.right);

        Gates.transform.position = PartMiddle.transform.position;
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

        PartDown.transform.position = PartMiddle.transform.position + new Vector3(0, 0, -120);
        PartDown.SetActive(true);

        ReplaceSide(Side.left);
        ReplaceSide(Side.right);

        Gates.transform.position = PartMiddle.transform.position;
        UpdateNavMesh();
    }
}
