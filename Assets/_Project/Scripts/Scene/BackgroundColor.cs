using UnityEngine;
using UnityEngine.Events;

public class BackgroundColor : Singleton<BackgroundColor>
{
    public event UnityAction<Color> OnColorChanged;
    
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

        OnColorChanged?.Invoke(color);
    }
}