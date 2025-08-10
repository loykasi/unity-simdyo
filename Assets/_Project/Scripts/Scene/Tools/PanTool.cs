using UnityEngine;
using UnityEngine.InputSystem;

public class PanTool : ITool
{
    private bool _onMouseDown;
    private Vector3 _origin;
    private Vector3 _pre;

    public void Disable()
    {
        
    }

    public void Enable()
    {
        
    }

    public void OnUpdate(Vector3 mousePosition)
    {
        Pan(mousePosition);
        Zoom();
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
            EngineManager.Instance.EditorCamera.transform.position += delta;
        }
    }

    public void Zoom()
    {
        if (ScreenInteractionUtils.IsOverUI())
        {
            return;
        }

        float scroll = Mouse.current.scroll.ReadValue().y;
        Vector2 limit = EngineManager.Instance.ZoomHeighLimit;
        float height = EngineManager.Instance.EditorCamera.orthographicSize;
        height = Mathf.Clamp(height - scroll, limit.x, limit.y);
        EngineManager.Instance.EditorCamera.orthographicSize = height;
    }
}