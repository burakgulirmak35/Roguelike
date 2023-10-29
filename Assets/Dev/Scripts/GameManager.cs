using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private Player player;

    [Header("Settings")]
    [SerializeField] public bool vSync_enable = false;
    [SerializeField] public bool FPS_limit = true;
    [SerializeField] public bool FPS_counter = true;

    private void Awake()
    {
        Instance = this;
        player = FindObjectOfType<Player>(true);
    }

    void Start()
    {
        FPS();
        Enviroment.Instance.CreateCity();
        player.StartGame();
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
