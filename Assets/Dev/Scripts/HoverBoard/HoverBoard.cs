using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HoverBoard : MonoBehaviour
{
    [SerializeField] private Animator HoverBoardAnim;
    [SerializeField] private GameObject BoardModel;
    [HideInInspector] public Transform BoardTransform;
    [Space]
    [SerializeField] private float FlyTime;
    [SerializeField] private float CoolDown;

    void Awake()
    {
        BoardTransform = transform;
        _propsLayer = LayerMask.GetMask("Props");
        _waitFly = new WaitForSeconds(FlyTime);
    }


    void Start()
    {
        UIManager.Instance.btn_HoverBoard.interactable = false;
        UIManager.Instance.btn_HoverBoard.onClick.AddListener(() => EnableHoverBoard());
        DisableHowerBoard();
    }

    private static readonly WaitForSeconds _waitTransition = new WaitForSeconds(0.5f);
    private WaitForSeconds _waitFly;

    private Coroutine HoverBoardActiveTimerCoro;
    private IEnumerator HoverBoardActiveTimer()
    {
        Player.Instance.playerState = PlayerState.CutScene;

        UIManager.Instance.btn_HoverBoard.interactable = false;
        UIManager.Instance.img_HoverBoard.fillAmount = 0;
        BoardModel.SetActive(true);
        Player.Instance.LeftLegIK.weight = 1;
        Player.Instance.RightLegIK.weight = 1;

        Player.Instance.PlayerTransform.DOLocalMove(Player.Instance.PlayerTransform.position + Vector3.up * 4, 0.5f);
        yield return _waitTransition;
        Player.Instance.playerState = PlayerState.HoverBoard;
        HoverBoardAnim.enabled = true;

        yield return _waitFly;
        Player.Instance.playerState = PlayerState.CutScene;

        if (isSafeLanding())
            Player.Instance.PlayerTransform.DOLocalMove(Player.Instance.PlayerTransform.position + Vector3.up * -4, 0.5f);
        else
            Player.Instance.PlayerTransform.DOLocalMove(Enviroment.Instance.CurrentCity.FindClosestSafePoint(), 0.5f);

        yield return _waitTransition;
        DisableHowerBoard();
    }

    public void EnableHoverBoard()
    {
        if (HoverBoardActiveTimerCoro != null) StopCoroutine(HoverBoardActiveTimerCoro);
        HoverBoardActiveTimerCoro = StartCoroutine(HoverBoardActiveTimer());
    }

    private void DisableHowerBoard()
    {
        Player.Instance.playerState = PlayerState.Normal;
        Player.Instance.LeftLegIK.weight = 0;
        Player.Instance.RightLegIK.weight = 0;
        HoverBoardAnim.enabled = false;
        BoardModel.SetActive(false);

        UIManager.Instance.img_HoverBoard.fillAmount = 0;
        UIManager.Instance.img_HoverBoard.DOFillAmount(1, CoolDown).OnComplete(() => UIManager.Instance.btn_HoverBoard.interactable = true);
    }

    private int _propsLayer;
    private readonly Collider[] hitColliders = new Collider[8];
    private Vector3 LandingPos;

    private bool isSafeLanding()
    {
        LandingPos = Player.Instance.PlayerTransform.position;
        LandingPos.y = 0;

        int count = Physics.OverlapSphereNonAlloc(LandingPos, 2, hitColliders, _propsLayer);
        return count == 0;
    }
}
