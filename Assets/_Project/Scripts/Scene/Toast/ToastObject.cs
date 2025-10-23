using System.Collections;
using TMPro;
using UnityEngine;
using PrimeTween;

public class ToastObject : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _rect;
    [SerializeField] private TextMeshProUGUI _label;

    private Vector2 _padding = new(20f, 20f);
    private Sequence _sequence;

    public void Show(string message, float fadeDuration, float duration)
    {
        _label.text = message;
        Vector2 size = _label.GetPreferredValues();
        _rect.sizeDelta = size + _padding;

        gameObject.SetActive(true);

        Tween fadeIn = Tween.Alpha(_canvasGroup, startValue: 0f, endValue: 1f, duration: fadeDuration);
        Tween fadeOut = Tween.Alpha(_canvasGroup, startValue: 1f, endValue: 0f, duration: fadeDuration);

        if (_sequence.isAlive)
        {
            _sequence.Stop();    
        }
        _sequence = Sequence.Create(useUnscaledTime: true)
            .Chain(fadeIn)
            .ChainDelay(duration)
            .Chain(fadeOut)
                .OnComplete(target: this, target => target.Hide());
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        Debug.Log(Time.frameCount);
    }
}