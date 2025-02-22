using UnityEngine;
using DG.Tweening;

public class UIAnimation : MonoBehaviour
{
    public enum UIAnimationType
    {
        FadeIn,
        ScaleUp,
        SlideInFromLeft,
        SlideInFromRight
    }

    [SerializeField] UIAnimationType animationType;
    [SerializeField] private float duration = 0.6f;
    [SerializeField] private float delay = 0f;
    [SerializeField] Ease easeType = Ease.OutBack;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Tween tween;
    private Vector3 originalScale;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    private void OnEnable()
    {
        PlayAnimation();
    }
    private void OnDisable()
    {
        StopAnimation();
    }
    public void PlayAnimation()
    {
        switch (animationType)
        {
            case UIAnimationType.FadeIn:
                canvasGroup.alpha = 0;
                tween = canvasGroup.DOFade(1, duration).SetEase(easeType).SetDelay(delay).SetUpdate(UpdateType.Normal, true);
                break;

            case UIAnimationType.ScaleUp:
                rectTransform.localScale = Vector3.zero;
                tween = rectTransform.DOScale(Vector3.one, duration).SetEase(easeType).SetDelay(delay).SetUpdate(UpdateType.Normal, true);
                break;
            case UIAnimationType.SlideInFromLeft:
                rectTransform.anchoredPosition = new Vector2(-Screen.width, rectTransform.anchoredPosition.y);
                tween = rectTransform.DOAnchorPosX(0, duration).SetEase(easeType).SetDelay(delay).SetUpdate(UpdateType.Normal, true);
                break;

            case UIAnimationType.SlideInFromRight:
                rectTransform.anchoredPosition = new Vector2(Screen.width, rectTransform.anchoredPosition.y);
                tween = rectTransform.DOAnchorPosX(0, duration).SetEase(easeType).SetDelay(delay).SetUpdate(UpdateType.Normal, true);
                break;
        }
    }
    private void StopAnimation()
    {
        tween?.Kill();
        tween = null;
    }
}
