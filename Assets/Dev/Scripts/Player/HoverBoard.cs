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
        UIManager.Instance.btn_HoverBoard.onClick.AddListener(() => EnableHoverBoard(true));

        UIManager.Instance.img_HoverBoard.fillAmount = 0;
        EnableHoverBoard(false);
    }


    private Coroutine HoverBoardActiveTimerCoro;
    private IEnumerator HoverBoardActiveTimer()
    {
        Player.Instance.playerState = PlayerState.HoverBoard;
        yield return new WaitForSeconds(15f);
        Player.Instance.playerState = PlayerState.Normal;
        Player.Instance.Holder.DOLocalMove(Vector3.zero, 1f).OnComplete(() => Player.Instance.healthSystem.isDamageble = true);
        UIManager.Instance.img_HoverBoard.DOFillAmount(0, 1f).OnComplete(() => EnableHoverBoard(false));
    }

    public void EnableHoverBoard(bool state)
    {
        if (state)
        {
            UIManager.Instance.btn_HoverBoard.interactable = false;
            UIManager.Instance.img_HoverBoard.fillAmount = 0;

            Board.SetActive(true);
            Player.Instance.LeftLegIK.weight = 1;
            Player.Instance.RightLegIK.weight = 1;
            HoverBoardAnim.enabled = true;

            Player.Instance.Holder.DOLocalMove(Vector3.up * 3, 1f).OnComplete(() => Player.Instance.healthSystem.isDamageble = false);

            if (HoverBoardActiveTimerCoro != null) StopCoroutine(HoverBoardActiveTimerCoro);
            HoverBoardActiveTimerCoro = StartCoroutine(HoverBoardActiveTimer());
        }
        else
        {
            Player.Instance.LeftLegIK.weight = 0;
            Player.Instance.RightLegIK.weight = 0;
            HoverBoardAnim.enabled = false;
            Board.SetActive(false);

            UIManager.Instance.img_HoverBoard.DOFillAmount(1, 5f).OnComplete(() => UIManager.Instance.btn_HoverBoard.interactable = true);
        }
    }
}
