using UnityEngine;
using UnityEngine.InputSystem;

public class PanTool : ITool
{
    public ToolType Type => ToolType.Pan;

    private bool _onMouseDown;
    private Vector3 _origin;

    private bool _isZooming;
    private float _targetHeight = 5f;

    public void Disable()
    {

    }

    public void Enable()
    {

    }

    public void OnUpdate(Vector3 mousePosition)
    {
        Zoom(mousePosition);
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
            EngineManager.Instance.EditorCamera.transform.position += delta;
        }
    }

    public void Zoom(Vector3 mousePosition)
    {
        if (ScreenInteractionUtils.IsOverUI())
        {
            return;
        }

        Camera camera = EngineManager.Instance.EditorCamera;
        float scroll = Mouse.current.scroll.ReadValue().y;

        if (!_isZooming && scroll != 0)
        {
            _targetHeight = camera.orthographicSize;
            _isZooming = true;
        }

        if (_isZooming && scroll == 0)
        {
            _isZooming = false;
        }

        Vector2 limit = EngineManager.Instance.ZoomHeighLimit;
        _targetHeight = Mathf.Clamp(_targetHeight - scroll * camera.orthographicSize / 5f, limit.x, limit.y); 

        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, _targetHeight, Time.unscaledDeltaTime * 10f);
        
        Vector3 offset = mousePosition - MouseWorldPositon();
        camera.transform.position += offset;
    }

    private Vector3 MouseWorldPositon()
    {
        Camera camera = EngineManager.Instance.EditorCamera;
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        Vector3 worldPoint = camera.ScreenToWorldPoint(mousePosition);
        worldPoint.z = 0;
        return worldPoint;
    }

}