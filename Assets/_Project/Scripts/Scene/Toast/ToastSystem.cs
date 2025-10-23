using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToastSystem : Singleton<ToastSystem>
{
    [SerializeField] private ToastObject _toast;
    [SerializeField] private float _fadeDuration = 0.3f;
    [SerializeField] private float _showDuration = 2f;

    private Coroutine _coroutine;

    public void Show(string message)
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _coroutine = StartCoroutine(DelayShow(message));
    }

    private void Update()
    {
        if (Keyboard.current.aKey.isPressed)
        {
            Show("Hello world!");
        }
    }

    private IEnumerator DelayShow(string message)
    {
        yield return null;
        _toast.Show(message, _fadeDuration, _showDuration);
        _coroutine = null;
    }
}