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
    }


    void Start()
    {
        UIManager.Instance.btn_HoverBoard.interactable = false;
        UIManager.Instance.btn_HoverBoard.onClick.AddListener(() => EnableHoverBoard());
        DisableHowerBoard();
    }

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
        yield return new WaitForSeconds(0.5f);
        Player.Instance.playerState = PlayerState.HoverBoard;
        HoverBoardAnim.enabled = true;

        yield return new WaitForSeconds(FlyTime);
        Player.Instance.playerState = PlayerState.CutScene;

        if (isSafeLanding())
            Player.Instance.PlayerTransform.DOLocalMove(Player.Instance.PlayerTransform.position + Vector3.up * -4, 0.5f);
        else
            Player.Instance.PlayerTransform.DOLocalMove(Enviroment.Instance.GetCurrentCity().FindClosestSafePoint(), 0.5f);

        yield return new WaitForSeconds(0.5f);
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

    private Vector3 LandingPos;
    private Collider[] hitColliders;
    private bool isSafeLanding()
    {
        LandingPos = Player.Instance.PlayerTransform.position;
        LandingPos.y = 0;

        hitColliders = Physics.OverlapSphere(LandingPos, 2);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].tag.Equals("Props"))
            {
                return false;
            }
        }
        return true;
    }
}
