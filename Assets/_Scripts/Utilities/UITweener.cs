using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public enum UIAnimationType
{
    Move,
    Scale,
    Fade
}

public class UITweener : MonoBehaviour
{
    [SerializeField] private UIAnimationType animationType;
    [SerializeField] private Ease easeType;
    [SerializeField] private float duration;
    [SerializeField] private float delay;

    [SerializeField] private int loopsCount;
    [SerializeField] private bool startReversed;

    [SerializeField] private Vector3 startValue;
    [SerializeField] private Vector3 finalValue;

    [SerializeField] private UnityEvent onShowCompleteCallback;
    [SerializeField] private UnityEvent onHideCompleteCallback;

    private bool _hasSetup;
    private bool _isReversed;
    private bool _isBusy;
    private RectTransform _rect;
    private CanvasGroup _canvasGroup;

    private void Setup()
    {
        if (_hasSetup)
            return;

        _rect = GetComponent<RectTransform>();

        switch (animationType)
        {
            case UIAnimationType.Move:
                _rect.anchoredPosition = startValue;
                if (startReversed)
                    _rect.anchoredPosition = finalValue;
                break;
            case UIAnimationType.Scale:
                _rect.localScale = startValue;
                if (startReversed)
                    _rect.localScale = finalValue;
                break;
            case UIAnimationType.Fade:
                _canvasGroup = GetComponent<CanvasGroup>();
                if (_canvasGroup == null)
                    _canvasGroup = gameObject.AddComponent<CanvasGroup>();
                _canvasGroup.alpha = startValue.x;
                if (startReversed)
                    _canvasGroup.alpha = finalValue.x;
                break;
        }

        _isReversed = false;
        if (startReversed)
            _isReversed = true;
        _hasSetup = true;
    }

    public void Show()
    {
        if (_isBusy)
            return;

        _isReversed = false;
        HandleTween();
    }

    public void Hide()
    {
        if (_isBusy)
            return;

        _isReversed = true;
        HandleTween();
    }

    public void Toggle()
    {
        if (_isBusy)
            return;

        _isReversed = !_isReversed;
        HandleTween();
    }

    private void HandleTween()
    {
        Setup();

        if (!_isReversed)
            gameObject.SetActive(true);

        _isBusy = true;
        switch (animationType)
        {
            case UIAnimationType.Fade:
                Fade();
                break;
            case UIAnimationType.Move:
                Move();
                break;
            case UIAnimationType.Scale:
                Scale();
                break;
        }
    }

    private void OnShowComplete()
    {
        if (onShowCompleteCallback != null)
            onShowCompleteCallback.Invoke();
    }

    private void OnHideComplete()
    {
        if (onHideCompleteCallback != null)
            onHideCompleteCallback.Invoke();
    }

    private void EndTween()
    {
        _isBusy = false;
    }

    private void Fade()
    {
        float targetFade = _isReversed ? startValue.x : finalValue.x;

        var tween = _canvasGroup.DOFade(targetFade, duration)
                                .SetDelay(delay)
                                .SetEase(easeType)
                                .SetUpdate(true);

        if (loopsCount != 0)
            tween.SetLoops(loopsCount);

        tween.onComplete += EndTween;

        if (_isReversed)
            tween.onComplete += OnHideComplete;
        else
            tween.onComplete += OnShowComplete;
    }

    private void Move()
    {
        Vector2 targetPos = _isReversed ? startValue : finalValue;

        var tween = _rect.DOAnchorPos(targetPos, duration)
                         .SetDelay(delay)
                         .SetEase(easeType)
                         .SetUpdate(true);

        if (loopsCount != 0)
            tween.SetLoops(loopsCount);

        tween.onComplete += EndTween;

        if (_isReversed)
            tween.onComplete += OnHideComplete;
        else
            tween.onComplete += OnShowComplete;
    }

    private void Scale()
    {
        Vector2 targetScale = _isReversed ? startValue : finalValue;

        var tween = transform.DOScale(targetScale, duration)
                             .SetDelay(delay)
                             .SetEase(easeType)
                             .SetUpdate(true);

        if (loopsCount != 0)
            tween.SetLoops(loopsCount);

        tween.onComplete += EndTween;

        if (_isReversed)
            tween.onComplete += OnHideComplete;
        else
            tween.onComplete += OnShowComplete;
    }
}
