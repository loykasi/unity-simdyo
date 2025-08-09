using UnityEngine;

public class CircleResizeHandler : IResize
{
    private CircleEntity _entity;
    private RectTransform _bound;

    private Vector3 _from;
    private Vector3 _direction;

    private Vector3 _topLeftDirection = new Vector3(-1f, 1f).normalized;
    private Vector3 _topRightDirection = new Vector3(1f, 1f).normalized;
    private Vector3 _bottomLeftDirection = new Vector3(-1f, -1f).normalized;
    private Vector3 _bottomRightDirection = new Vector3(1f, -1f).normalized;

    public void Init(SceneEntity entity, RectTransform bound)
    {
        _entity = (CircleEntity)entity;
        _bound = bound;

        UpdateBound();
    }

    public void UpdateBound()
    {
        _entity.transform.GetPositionAndRotation(out Vector3 position, out Quaternion rotation);
        float diameter = _entity.Radius * 2;

        Camera camera = EngineManager.Instance.EditorCamera;
        float scale = Screen.height / (camera.orthographicSize * 2);

        _bound.position = camera.WorldToScreenPoint(position);
        _bound.sizeDelta = diameter * scale * Vector2.one;
    }

    public void BeginResize(BoundsHandleDirection direction)
    {
        _from = _entity.transform.position;
        _direction = GetDirection(direction);
    }

    public void Resize(BoundsHandleDirection direction, Vector3 mousePosition)
    {
        Camera camera = EngineManager.Instance.EditorCamera;

        Vector3 position = camera.ScreenToWorldPoint(mousePosition);
        position.z = 0;

        Vector3 to;

        Vector3 dragVector = position - _from;
        float dot = Vector3.Dot(_direction, dragVector);
        to = GetToPoint(direction, _from + dot * _direction);
        // to = _from + dot * _direction;
        
        Debug.DrawRay(to, Vector3.up, Color.black);
        Debug.DrawRay(to, Vector3.down, Color.black);
        Debug.DrawRay(to, Vector3.right, Color.black);
        Debug.DrawRay(to, Vector3.left, Color.black);

        _entity.UpdateCircle(_from, to);
    }

    private Vector3 GetDirection(BoundsHandleDirection direction)
    {
        return direction switch
        {
            BoundsHandleDirection.Right => Vector3.right,
            BoundsHandleDirection.Left => Vector3.left,
            BoundsHandleDirection.Top => Vector3.up,
            BoundsHandleDirection.Bottom => Vector3.down,
            BoundsHandleDirection.TopLeft => _topLeftDirection,
            BoundsHandleDirection.TopRight => _topRightDirection,
            BoundsHandleDirection.BottomLeft => _bottomLeftDirection,
            BoundsHandleDirection.BottomRight => _bottomRightDirection,
            _ => Vector3.zero,
        };
    }

    private Vector3 GetToPoint(BoundsHandleDirection direction, Vector3 point)
    {
        return direction switch
        {
            BoundsHandleDirection.TopLeft => Vector3Utils.ProjectOnVector(point, _from, Vector3.right),
            BoundsHandleDirection.TopRight => Vector3Utils.ProjectOnVector(point, _from, Vector3.right),
            BoundsHandleDirection.BottomLeft => Vector3Utils.ProjectOnVector(point, _from, Vector3.right),
            BoundsHandleDirection.BottomRight => Vector3Utils.ProjectOnVector(point, _from, Vector3.right),
            _ => point,
        };
    }
}