using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Performance Settings")]
    [SerializeField] private bool vSync_enable = false;
    [Range(-1, 60)][SerializeField] private int FPS_target = 60;
    [SerializeField] private bool FPS_counter = true;

    public static GameManager Instance { get; private set; }
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        FPS();
        PoolManager.Instance.GeneratePools();
        Enviroment.Instance.CreateCity();
        Player.Instance.StartGame();
        Spawner.Instance.StartGame();
    }

    #region Effects

    public void SlowMotion()
    {
        if (slowMotionCoro != null)
        {
            StopCoroutine(slowMotionCoro);
        }
        slowMotionCoro = StartCoroutine(SlowMotionTimer());
    }

    private Coroutine slowMotionCoro;
    private bool isSlowMotion;
    private IEnumerator SlowMotionTimer()
    {
        isSlowMotion = true;
        Time.timeScale = PlayerData.Instance.SlowMotionPercent;
        yield return new WaitForSeconds(PlayerData.Instance.SlowMotionTime);
        isSlowMotion = false;
        if (Player.Instance.healthSystem.isAlive)
        {
            Time.timeScale = 1;
        }
    }

    #endregion

    #region Timee
    public void FreezeGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        if (isSlowMotion)
        {
            Time.timeScale = PlayerData.Instance.SlowMotionPercent;
            return;
        }
        Time.timeScale = 1;
    }
    #endregion

    #region MainMenu
    public void MainMenu()
    {
        Player.Instance.ClearInputs();
        PlayerData.Instance.ResetData();
        StartCoroutine(MainMenuTimer());
    }

    private IEnumerator MainMenuTimer()
    {
        UIManager.Instance.panelFadeInOut.FadeOut(1);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("MainMenu");
    }
    #endregion

    #region RestartGame
    public void RestartGame()
    {
        Player.Instance.ClearInputs();
        PlayerData.Instance.ResetData();
        StartCoroutine(RestartGameTimer());
    }

    private IEnumerator RestartGameTimer()
    {
        UIManager.Instance.panelFadeInOut.FadeOut(1);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion

    #region ContinueGame
    public void ContinueGame()
    {
        UIManager.Instance.EnablePanelPlayerDead(false);
        Player.Instance.ReBorn();
        CameraManager.Instance.ZoomTo(1);
        ResumeGame();
    }
    #endregion

    private void FPS()
    {
        if (!vSync_enable) { QualitySettings.vSyncCount = 0; }
        Application.targetFrameRate = FPS_target;
        FindObjectOfType<FPSCounter>(true).gameObject.SetActive(FPS_counter);
    }
}
