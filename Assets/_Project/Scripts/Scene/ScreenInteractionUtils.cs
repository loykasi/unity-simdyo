using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public static class ScreenInteractionUtils
{
    private static readonly List<RaycastResult> _results = new();
    private static readonly PointerEventData _pointerData = new(EventSystem.current);

    public static bool IsOverUI()
    {
        _pointerData.position = Mouse.current.position.ReadValue();
        EventSystem.current.RaycastAll(_pointerData, _results);
        return _results.Count > 0;
    }
}