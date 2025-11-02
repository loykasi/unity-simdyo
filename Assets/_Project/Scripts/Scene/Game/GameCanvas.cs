using UnityEngine;
using UnityEngine.Events;

public class GameCanvas : Singleton<GameCanvas>
{
    public event UnityAction OnResized;
    
    public Vector2 CanvasSize => new(Screen.width, Screen.height);
    private Vector2 _previousSize;

    private void Start()
    {
        _previousSize = CanvasSize;
    }

    private void Update()
    {
        if (_previousSize.x != Screen.width || _previousSize.y != Screen.height)
        {
            _previousSize = CanvasSize;
            OnResized?.Invoke();
        }
    }
}