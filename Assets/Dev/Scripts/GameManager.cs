using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Performance Settings")]
    [SerializeField] private bool vSync_enable = false;
    [SerializeField] private bool FPS_limit = true;
    [Range(30, 60)][SerializeField] private int FPS_target = 60;
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

        if (FPS_limit)
        {
            Application.targetFrameRate = FPS_target;
        }
        else
        {
            Application.targetFrameRate = -1;
        }

        FindObjectOfType<FPSCounter>(true).gameObject.SetActive(FPS_counter);
    }
}
