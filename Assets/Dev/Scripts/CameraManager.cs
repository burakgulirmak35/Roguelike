using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(CinemachineCamera))]
public class CameraManager : MonoBehaviour
{
    [Header("CameraSettings")]
    [SerializeField] private Vector3 DeathPos;
    [SerializeField] private List<Vector3> FollowPoints = new List<Vector3>();

    private CinemachineCamera cinemachineCamera;
    private CinemachineFollow cinemachineFollow;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;

    [Header("RifleShake")]
    [SerializeField][Range(0, 5)] private float shakeCameraTime = 0.1f;
    [SerializeField][Range(0, 5)] private float shakeCameraIntensity = 1f;

    public static CameraManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;

        cinemachineCamera = GetComponent<CinemachineCamera>();
        cinemachineFollow = cinemachineCamera.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachineFollow;
        cinemachineBasicMultiChannelPerlin = cinemachineCamera.GetCinemachineComponent(CinemachineCore.Stage.Noise) as CinemachineBasicMultiChannelPerlin;
        cinemachineCamera.Priority.Value = 1;
    }

    void Start()
    {
        cinemachineCamera.Follow = Player.Instance.PlayerTransform;
    }

    #region Zoom
    private int CurrentCamIndex;
    public void CamChangePos()
    {
        CurrentCamIndex++;
        if (CurrentCamIndex >= FollowPoints.Count) CurrentCamIndex = 0;
        DOTween.To(() => cinemachineFollow.FollowOffset, x => cinemachineFollow.FollowOffset = x, FollowPoints[CurrentCamIndex], 0.5f);
    }

    public void CamDefaultPos()
    {
        CurrentCamIndex = 0;
        DOTween.To(() => cinemachineFollow.FollowOffset, x => cinemachineFollow.FollowOffset = x, FollowPoints[CurrentCamIndex], 0.5f);
    }

    public void CamDeathPos()
    {
        DOTween.To(() => cinemachineFollow.FollowOffset, x => cinemachineFollow.FollowOffset = x, DeathPos, 0.5f);
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
        cinemachineBasicMultiChannelPerlin.AmplitudeGain = shakeCameraIntensity;
        yield return new WaitForSeconds(shakeCameraTime);
        cinemachineBasicMultiChannelPerlin.AmplitudeGain = 0;
    }

}
