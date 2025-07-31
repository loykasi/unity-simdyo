using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public static class ScreenInteractionUtils
{
    private static readonly List<RaycastResult> _results = new();

    public static bool IsOverUI()
    {
        var data = new PointerEventData(EventSystem.current)
        {
            position = Mouse.current.position.ReadValue()
        };
        EventSystem.current.RaycastAll(data, _results);
        return _results.Count > 0;
    }
}