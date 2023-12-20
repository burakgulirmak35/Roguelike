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


    private Coroutine HoverBoardCoro;
    private IEnumerator HoverBoardTimer()
    {
        yield return new WaitForSeconds(15f);
        Player.Instance.Holder.DOLocalMove(Vector3.zero, 1f).OnComplete(() => Player.Instance.healthSystem.isDamageble = true);
        UIManager.Instance.img_HoverBoard.DOFillAmount(0, 1f).OnComplete(() => EnableHoverBoard(false));
    }

    public void EnableHoverBoard(bool state)
    {
        if (state)
        {
            UIManager.Instance.btn_HoverBoard.interactable = false;

            Board.SetActive(true);
            Player.Instance.LeftLegIK.weight = 1;
            Player.Instance.RightLegIK.weight = 1;
            HoverBoardAnim.enabled = true;

            Player.Instance.Holder.DOLocalMove(Vector3.up * 3, 1f).OnComplete(() => Player.Instance.healthSystem.isDamageble = false);

            if (HoverBoardCoro != null) StopCoroutine(HoverBoardCoro);
            HoverBoardCoro = StartCoroutine(HoverBoardTimer());
        }
        else
        {
            Player.Instance.LeftLegIK.weight = 0;
            Player.Instance.RightLegIK.weight = 0;
            HoverBoardAnim.enabled = false;
            Board.SetActive(false);

            UIManager.Instance.img_HoverBoard.DOFillAmount(1, 15f).OnComplete(() => UIManager.Instance.btn_HoverBoard.interactable = true);
        }
    }
}
