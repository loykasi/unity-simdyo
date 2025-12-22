using UnityEngine;

public class LoadingScreen : Singleton<LoadingScreen>
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _failureCanvas;

    public void Toggle(bool value)
    {
        _canvas.SetActive(value);
    }

    public void ToggleFailureScreen(bool value)
    {
        _failureCanvas.SetActive(value);
    }
}