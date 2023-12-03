using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    private Camera MainCam;
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;

    [Header("RifleShake")]
    [SerializeField][Range(0, 5)] private float shakeCameraTime = 0.1f;
    [SerializeField][Range(0, 5)] private float shakeCameraIntensity = 1f;

    public static CameraManager Instance { get; private set; }
    void Awake()
    {
        Instance = this;
        MainCam = Camera.main;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera()
    {
        if (ShakeCameraCoro != null)
        {
            StopCoroutine(ShakeCameraCoro);
        }
        ShakeCameraCoro = StartCoroutine(ShakeCameraTimer());
    }

    private Coroutine ShakeCameraCoro;
    private IEnumerator ShakeCameraTimer()
    {
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = shakeCameraIntensity;
        yield return new WaitForSeconds(shakeCameraTime);
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
    }

}
