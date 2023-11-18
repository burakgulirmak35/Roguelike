using System.Collections;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txt_FPS;
    private float frequency = 0.5f;
    private int FPS;

    private int lastFrameCount;
    private float lastTime;
    private float timeSpan;
    private int frameCount;

    private void OnEnable()
    {
        if (FPSCorotine != null) { StopCoroutine(FPSCorotine); }

        FPSCorotine = StartCoroutine(CalculateFps());
        txt_FPS.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        if (FPSCorotine != null) { StopCoroutine(FPSCorotine); }
        txt_FPS.gameObject.SetActive(false);
    }


    private Coroutine FPSCorotine;
    private IEnumerator CalculateFps()
    {
        while (true)
        {
            lastFrameCount = Time.frameCount;
            lastTime = Time.realtimeSinceStartup;
            yield return new WaitForSeconds(frequency);

            timeSpan = Time.realtimeSinceStartup - lastTime;
            frameCount = Time.frameCount - lastFrameCount;

            FPS = Mathf.RoundToInt(frameCount / timeSpan);
            txt_FPS.text = "FPS: " + FPS.ToString();
        }
    }
}
