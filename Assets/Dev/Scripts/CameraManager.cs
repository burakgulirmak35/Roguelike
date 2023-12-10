using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    [Header("CameraSettings")]
    [SerializeField] private List<Vector3> FollowPoints = new List<Vector3>();

    private Camera MainCam;
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineTransposer cinemachineTransposer;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;

    [Header("RifleShake")]
    [SerializeField][Range(0, 5)] private float shakeCameraTime = 0.1f;
    [SerializeField][Range(0, 5)] private float shakeCameraIntensity = 1f;

    public static CameraManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        MainCam = Camera.main;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    #region Zoom
    [HideInInspector] public int CurrentCamIndex;
    public void ZoomTo(int Index)
    {
        CurrentCamIndex = Index;
        if (Index >= FollowPoints.Count) Index = FollowPoints.Count - 1;
        else if (Index <= 0) Index = 0;

        DOTween.To(() => cinemachineTransposer.m_FollowOffset, x => cinemachineTransposer.m_FollowOffset = x, FollowPoints[Index], 0.5f);
    }
    #endregion

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
