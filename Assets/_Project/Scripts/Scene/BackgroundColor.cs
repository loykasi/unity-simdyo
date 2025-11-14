using UnityEngine;

public class BackgroundColor : Singleton<BackgroundColor>
{
    [SerializeField] private Camera _editorCamera;
    [SerializeField] private Camera _gameCamera;

    public void SetColor(Color color)
    {
        if (_editorCamera != null)
        {
            _editorCamera.backgroundColor = color;   
        }
        if (_gameCamera != null)
        {
            _gameCamera.backgroundColor = color;
        }
    }
}