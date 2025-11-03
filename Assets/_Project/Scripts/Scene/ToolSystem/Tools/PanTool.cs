using UnityEngine;
using UnityEngine.InputSystem;

public class PanTool : ITool
{
    public virtual ToolType Type => ToolType.Pan;

    protected bool _onMouseLeftDown;
    protected bool _onMouseRightDown;
    private Vector3 _origin;

    private bool _isZooming;
    private float TargetHeight
    {
        get => EngineManager.Instance.EditorCameraHeight;
        set => EngineManager.Instance.EditorCameraHeight = value;
    }

    private float _startZoom = 5f;
    private float _zoomTime = 1f;

    public virtual void Disable()
    {

    }

    public virtual void Enable()
    {

    }

    public virtual void OnUpdate()
    {
        Zoom();
        HandlePanLeftMouse();
        HandlePanRightMouse();
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

    protected virtual void Zoom()
    {
        Camera camera = EngineManager.Instance.EditorCamera;
        Vector3 mousePosition = GetMouseWorldPositon();

        if (_zoomTime < 1)
        {
            _zoomTime += Time.unscaledDeltaTime * ToolManagement.Instance.PanZoomSpeed;
        }
        else
        {
            _zoomTime = 1;
        }

        camera.orthographicSize = Mathf.Lerp(_startZoom, TargetHeight, EaseZoom(_zoomTime));

        Vector3 offset = mousePosition - GetMouseWorldPositon();
        camera.transform.position += offset;

        if (ScreenInteractionUtils.IsOverUI())
        {
            return;
        }

        float scroll = Mouse.current.scroll.ReadValue().y;
        if (!_isZooming && scroll != 0)
        {
            _isZooming = true;

            _startZoom = camera.orthographicSize;

            Vector2 limit = EngineManager.Instance.ZoomHeighLimit;
            TargetHeight = Mathf.Clamp(camera.orthographicSize - scroll * camera.orthographicSize / 5f, limit.x, limit.y);
            _zoomTime = 0;
        }

        if (_isZooming && scroll == 0)
        {
            _isZooming = false;
        }
    }
    
    private float EaseZoom(float t)
    {
        return 1 - Mathf.Pow(1 - t, 3);
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