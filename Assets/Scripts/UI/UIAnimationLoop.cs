using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIAnimationLoop : MonoBehaviour
{
    [SerializeField] private float multiplier = 1.2f;
    [SerializeField] private float duration = 0.6f;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Tween tween;
    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
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
    private void PlayAnimation()
    {
        tween = rectTransform.DOScale(originalScale * multiplier, duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine).SetUpdate(UpdateType.Normal, true);
    }
    private void StopAnimation()
    {
        tween?.Kill();
        tween = null;
    }
}
