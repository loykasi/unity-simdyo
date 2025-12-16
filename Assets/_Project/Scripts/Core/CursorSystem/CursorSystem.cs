using System.Collections.Generic;
using UnityEngine;

public class CursorSystem : Singleton<CursorSystem>
{
    [SerializeField] private CursorSO _cursorData;
    private CursorType _currentType;
    private CursorType _targetType;
    private int _priority = 0;
    private bool _shouldSetCursor;

    private void Start()
    {
        Texture2D cursor = _cursorData.Get(CursorType.Default);
        Cursor.SetCursor(cursor, new Vector2(8f, 8f), CursorMode.Auto);
    }

    private void LateUpdate()
    {
        if (!_shouldSetCursor || _targetType == _currentType)
        {
            _shouldSetCursor = false;
            return;
        }

        _currentType = _targetType;
        Texture2D cursor = _cursorData.Get(_currentType);
        Cursor.SetCursor(cursor, new Vector2(8f, 8f), CursorMode.Auto);
        _priority = 0;
        _shouldSetCursor = false;
    }

    public void SetCursor(CursorType type, int priority = 0)
    {
        if (priority < _priority)
        {
            return;
        }
        
        _priority = priority;
        _targetType = type;
        _shouldSetCursor = true;
    }

    public void ToDefault(int priority = 0)
    {
        if (_priority <= priority)
        {
            _priority = 0;
            SetCursor(CursorType.Default, 0);
        }
    }
}