using UnityEngine;
using UnityEngine.InputSystem;

public class PanTool : ITool
{
    private bool _onMouseDown;
    private Vector3 _origin;
    private Vector3 _pre;

    public void OnUpdate(Vector3 mousePosition)
    {
        Pan(mousePosition);
    }

    public void Pan(Vector3 mousePosition)
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !ScreenInteractionUtils.IsOverUI())
        {
            _origin = mousePosition;
            _onMouseDown = true;
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _onMouseDown = false;
        }

        if (_onMouseDown)
        {
            Vector3 delta = _origin - mousePosition;
            EngineManager.Instance.Camera.transform.position += delta;
        }
    }
}