using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PanelFadeInOut : MonoBehaviour
{
    [SerializeField] private Image FadeImage;
    [SerializeField] private CanvasGroup canvasGroup;

    public void FadeIn(float _time)
    {
        canvasGroup.alpha = 1;
        FadeImage.enabled = true;
        DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 0, _time).SetEase(Ease.Linear)
        .OnComplete(() => FadeImage.enabled = false);
    }

    public void FadeOut(float _time)
    {
        canvasGroup.alpha = 0;
        FadeImage.enabled = true;
        DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 1, _time).SetEase(Ease.Linear);
    }
}
