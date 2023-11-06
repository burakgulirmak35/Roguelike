using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Performance Settings")]
    [SerializeField] private bool vSync_enable = false;
    [SerializeField] private bool FPS_limit = true;
    [SerializeField] private bool FPS_counter = true;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Start()
    {
        FPS();
        PoolManager.Instance.GeneratePools();
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
