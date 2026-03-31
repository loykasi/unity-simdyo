using UnityEngine;
using UnityEngine.InputSystem;

public class PanTool : BaseTool
{
    public override ToolType Type => ToolType.Pan;

    protected bool _onMouseLeftDown;
    protected bool _onMouseRightDown;
    private Vector3 _origin;

    private float TargetHeight
    {
        get => EngineManager.Instance.EditorCameraHeight;
        set => EngineManager.Instance.EditorCameraHeight = value;
    }

    private Vector3 _startMousePosition;

    public override void Disable()
    {

    }

    public override void Enable()
    {

    }

    public override void OnUpdate()
    {
        Zoom();
        HandlePanLeftMouse();
        HandlePanRightMouse();
        HandleSelection();
        HandleContextMenu();
    }

    protected virtual void HandleSelection()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (ScreenInteractionUtils.IsOverUI())
            {
                return;
            }

            _startMousePosition = Mouse.current.position.ReadValue();
        }
        
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if (ScreenInteractionUtils.IsOverUI())
            {
                return;
            }
            
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            if (_startMousePosition != mousePosition)
            {
                return;
            }

            ObjectManager.Instance.Select(mousePosition);
        }
    }

    protected virtual void HandleContextMenu()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (ScreenInteractionUtils.IsOverUI())
            {
                return;
            }

            _startMousePosition = Mouse.current.position.ReadValue();
        }
        
        if (Mouse.current.rightButton.wasReleasedThisFrame)
        {
            if (ScreenInteractionUtils.IsOverUI())
            {
                return;
            }
            
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            if (_startMousePosition != mousePosition)
            {
                return;
            }

            ObjectManager.Instance.Select(mousePosition);
            EntityContextMenuController.Instance.Open();
        }
    }

    protected virtual void HandlePanLeftMouse()
    {
        if (_onMouseRightDown)
        {
            return;
        }

        Vector3 mousePosition = GetMouseWorldPositon();
        if (!_onMouseLeftDown && Mouse.current.leftButton.wasPressedThisFrame && !ScreenInteractionUtils.IsOverUI())
        {
            _origin = mousePosition;
            _onMouseLeftDown = true;
        }

        if (_onMouseLeftDown && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _onMouseLeftDown = false;
        }

        if (_onMouseLeftDown)
        {
            Vector3 delta = _origin - mousePosition;
            EngineManager.Instance.EditorCamera.transform.position += delta;
        }
    }
    
    protected virtual void HandlePanRightMouse()
    {
        if (_onMouseLeftDown)
        {
            return;
        }

        Vector3 mousePosition = GetMouseWorldPositon();
        if (!_onMouseRightDown && Mouse.current.rightButton.wasPressedThisFrame && !ScreenInteractionUtils.IsOverUI())
        {
            _origin = mousePosition;
            _onMouseRightDown = true;
        }

        if (_onMouseRightDown && Mouse.current.rightButton.wasReleasedThisFrame)
        {
            _onMouseRightDown = false;
        }

        if (_onMouseRightDown)
        {
            Vector3 delta = _origin - mousePosition;
            EngineManager.Instance.EditorCamera.transform.position += delta;
        }
    }

    private float ExponentialDecay(float a, float b, float decay, float dt)
    {
        return b + (a-b) * Mathf.Exp(-decay * dt);
    }

    protected virtual void Zoom()
    {
        Camera camera = EngineManager.Instance.EditorCamera;
        Vector3 mousePosition = GetMouseWorldPositon();

        camera.orthographicSize = ExponentialDecay(camera.orthographicSize, TargetHeight, EngineManager.Instance.SmoothFactor, Time.unscaledDeltaTime);

        Vector3 offset = mousePosition - GetMouseWorldPositon();
        camera.transform.position += offset;

        if (ScreenInteractionUtils.IsOverUI())
        {
            return;
        }

        float scroll = Mouse.current.scroll.ReadValue().y;
        if (scroll != 0)
        {
            Vector2 limit = EngineManager.Instance.ZoomHeighLimit;

            float zoomValue = 1f + EngineManager.Instance.ZoomSpeed;
            float zoomFactor = Mathf.Sign(scroll) > 0 ? 1f / zoomValue : zoomValue;
            TargetHeight = Mathf.Clamp(TargetHeight * zoomFactor, limit.x, limit.y);
        }
    }

    protected Vector3 GetMouseWorldPositon()
    {
        Camera camera = EngineManager.Instance.EditorCamera;
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        Vector3 worldPoint = camera.ScreenToWorldPoint(mousePosition);
        worldPoint.z = 0;
        return worldPoint;
    }
}