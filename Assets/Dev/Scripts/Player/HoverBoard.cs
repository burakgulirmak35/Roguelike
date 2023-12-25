using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HoverBoard : MonoBehaviour
{
    [SerializeField] private Animator HoverBoardAnim;
    [SerializeField] private GameObject Board;

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
        Board.SetActive(true);
        Player.Instance.LeftLegIK.weight = 1;
        Player.Instance.RightLegIK.weight = 1;
        Player.Instance.PlayerTransform.DOLocalMove(Player.Instance.PlayerTransform.position + Vector3.up * 3, 1f);

        yield return new WaitForSeconds(1f);
        Player.Instance.playerState = PlayerState.HoverBoard;

        yield return new WaitForSeconds(15f);
        Player.Instance.playerState = PlayerState.CutScene;

        Player.Instance.PlayerTransform.DOLocalMove(Player.Instance.PlayerTransform.position + Vector3.up * -3, 1f);

        yield return new WaitForSeconds(1f);
        Player.Instance.playerState = PlayerState.Normal;

        DisableHowerBoard();
    }

    public void EnableHoverBoard()
    {
        if (HoverBoardActiveTimerCoro != null) StopCoroutine(HoverBoardActiveTimerCoro);
        HoverBoardActiveTimerCoro = StartCoroutine(HoverBoardActiveTimer());
    }

    private void DisableHowerBoard()
    {
        Player.Instance.LeftLegIK.weight = 0;
        Player.Instance.RightLegIK.weight = 0;
        HoverBoardAnim.enabled = false;
        Board.SetActive(false);

        UIManager.Instance.img_HoverBoard.fillAmount = 0;
        UIManager.Instance.img_HoverBoard.DOFillAmount(1, 5f).OnComplete(() => UIManager.Instance.btn_HoverBoard.interactable = true);
    }
}
