using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("LevelSettings")]
    public List<int> expPerLevel = new List<int>();

    [Header("Performance Settings")]
    [SerializeField] private bool vSync_enable = false;
    [SerializeField] private bool FPS_limit = true;
    [SerializeField] private bool FPS_counter = true;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        FPS();
        Enviroment.Instance.CreateCity();
        Player.Instance.StartGame();
        Spawner.Instance.StartGame();
    }

    private void FPS()
    {
        if (!vSync_enable) { QualitySettings.vSyncCount = 0; }

        if (FPS_limit) { Application.targetFrameRate = 60; }
        else { Application.targetFrameRate = -1; }

        if (FPS_counter) { FindObjectOfType<FPSCounter>(true).gameObject.SetActive(FPS_counter); }

    }
}
