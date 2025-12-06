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

    // private void Update()
    // {
    //     Debug.Log($"Priority: {_priority}");
    // }

    public void SetCursor(CursorType type, int priority = 0)
    {
        if (priority < _priority)
        {
            return;
        }

        _priority = priority;
        Texture2D cursor = _cursorData.Get(type);
        Cursor.SetCursor(cursor, new Vector2(8f, 8f), CursorMode.Auto);
        Debug.Log($"{priority} | set cursor {type}");
    }

    public void ToDefault(int priority = 0)
    {
        Debug.Log($"{priority} | to default");
        if (_priority <= priority)
        {
            _priority = 0;
            SetCursor(CursorType.Default);
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