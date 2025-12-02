using System.Collections.Generic;
using UnityEngine;

public class CursorSystem : Singleton<CursorSystem>
{
    [SerializeField] private CursorSO _cursorData;
    private int _priority = 0;

    private void Start()
    {
        SetCursor(CursorType.Default);
    }

    public void SetCursor(CursorType type, int priority = 0)
    {
        if (priority < _priority)
        {
            return;
        }

        _priority = priority;
        Texture2D cursor = _cursorData.Get(type);
        Cursor.SetCursor(cursor, new Vector2(8f, 8f), CursorMode.Auto);
    }

    public void ToDefault(int priority = 0)
    {
        if (_priority == priority)
        {
            SetCursor(CursorType.Default);
            _priority = 0;
        }
    }

    // public void SetCursor(object target, CursorType type, int priority = 0)
    // {
        
    // }

    // public struct CursorState
    // {
    //     public object target;
    //     public CursorType type;
    //     public int priority;
    // }
}