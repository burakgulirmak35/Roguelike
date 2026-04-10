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

    private WaitForSeconds waitOneSecond;

    public static GameManager Instance { get; private set; }
    void Awake()
    {
        Instance = this;
        waitOneSecond = new WaitForSeconds(1f);
    }

    void Start()
    {
        FPS();
        PoolManager.Instance.GeneratePools();
        Player.Instance.StartGame();
        Spawner.Instance.StartGame();
    }

    #region Time
    public void FreezeGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
    #endregion

    private void OnApplicationQuit()
    {
        PlayerData.Instance.SaveScore();
    }

    #region MainMenu
    public void MainMenu()
    {
        Player.Instance.ClearInputs();
        PlayerData.Instance.SaveScore();
        PlayerData.Instance.ResetData();
        StartCoroutine(MainMenuTimer());
    }

    private IEnumerator MainMenuTimer()
    {
        UIManager.Instance.panelFadeInOut.FadeOut(1);
        yield return waitOneSecond;
        SceneManager.LoadScene("MainMenu");
    }
    #endregion

    #region RestartGame
    public void RestartGame()
    {
        Player.Instance.ClearInputs();
        PlayerData.Instance.SaveScore();
        PlayerData.Instance.ResetData();
        StartCoroutine(RestartGameTimer());
    }

    private IEnumerator RestartGameTimer()
    {
        UIManager.Instance.panelFadeInOut.FadeOut(1);
        yield return waitOneSecond;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion

    #region ContinueGame
    public void ContinueGame()
    {
        UIManager.Instance.EnablePanelPlayerDead(false);
        Player.Instance.ReBorn();
        CameraManager.Instance.CamDefaultPos();
        ResumeGame();
    }
    #endregion

    private void FPS()
    {
        if (!vSync_enable) { QualitySettings.vSyncCount = 0; }
        Application.targetFrameRate = FPS_target;
        FindFirstObjectByType<FPSCounter>(FindObjectsInactive.Include).gameObject.SetActive(FPS_counter);
    }
}
