using UnityEngine;

public class LoadingScreen : Singleton<LoadingScreen>
{
    [SerializeField] private GameObject _canvas;

    public void Toggle(bool value)
    {
        _canvas.SetActive(value);
    }
}