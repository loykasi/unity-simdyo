using UnityEngine;

public class ResizePolygon: IResize
{
    private PolygonEntity _entity;
    private RectTransform _bound;

    private Vector3 _pivotPoint;
    private Vector3 _fromPoint;

    public void Init(SceneEntity entity, RectTransform bound)
    {
        _entity = (PolygonEntity)entity;
        _bound = bound;

        UpdateBound();
    }

    public void UpdateBound()
    {
        // _entity.transform.GetPositionAndRotation(out Vector3 position, out Quaternion rotation);
        Vector2 size = new(_entity.Width, _entity.Height);

        Camera camera = EngineManager.Instance.EditorCamera;
        float scale = Screen.height / (camera.orthographicSize * 2);

        _bound.position = camera.WorldToScreenPoint(_entity.Bounds.center);
        _bound.sizeDelta = scale * size / EngineManager.Instance.CanvasScale;
    }

    public void BeginResize(BoundsHandleDirection direction)
    {
        _entity.ApplyRotation();
        _pivotPoint = GetPivotPoint(direction);
        _fromPoint = GetFromPoint(direction);
    }

    public void Resize(BoundsHandleDirection direction, Vector3 mousePosition)
    {
        Camera camera = EngineManager.Instance.EditorCamera;

        Vector3 position = camera.ScreenToWorldPoint(mousePosition);
        position.z = 0;

        position = Vector3Utils.GetGridPosition(position);

        Vector3 directionVector = GetDirection(direction);
        Vector3 to;

        if (directionVector != Vector3.zero)
        {
            Vector3 dragVector = position - _pivotPoint;
            float dot = Vector3.Dot(directionVector, dragVector);
            Vector3 newPosition = _pivotPoint + dot * directionVector;

            to = GetToPoint(direction, newPosition);
        }
        else
        {
            to = position;
        }

        Debug.DrawRay(_fromPoint, Vector3.up, Color.red);
        Debug.DrawRay(_fromPoint, Vector3.down, Color.red);
        Debug.DrawRay(_fromPoint, Vector3.right, Color.red);
        Debug.DrawRay(_fromPoint, Vector3.left, Color.red);

        Debug.DrawRay(to, Vector3.up, Color.black);
        Debug.DrawRay(to, Vector3.down, Color.black);
        Debug.DrawRay(to, Vector3.right, Color.black);
        Debug.DrawRay(to, Vector3.left, Color.black);

        _entity.UpdateSize(_fromPoint, to);
    }

    private Vector3 GetDirection(BoundsHandleDirection direction)
    {
        return direction switch
        {
            BoundsHandleDirection.Right => _entity.transform.right,
            BoundsHandleDirection.Left => - _entity.transform.right,
            BoundsHandleDirection.Top => _entity.transform.up,
            BoundsHandleDirection.Bottom => - _entity.transform.up,
            _ => Vector3.zero,
        };
    }

    private Vector3 GetPivotPoint(BoundsHandleDirection direction)
    {
        return direction switch
        {
            BoundsHandleDirection.Right => _entity.Right,
            BoundsHandleDirection.Left => _entity.Left,
            BoundsHandleDirection.Top => _entity.Top,
            BoundsHandleDirection.Bottom => _entity.Bottom,
            _ => Vector3.zero,
        };
    }

    private Vector3 GetFromPoint(BoundsHandleDirection direction)
    {
        return direction switch
        {
            BoundsHandleDirection.Right => _entity.TopLeft,
            BoundsHandleDirection.Left => _entity.TopRight,
            BoundsHandleDirection.Top => _entity.BottomLeft,
            BoundsHandleDirection.Bottom => _entity.TopLeft,

            BoundsHandleDirection.TopLeft => _entity.BottomRight,
            BoundsHandleDirection.TopRight => _entity.BottomLeft,
            BoundsHandleDirection.BottomLeft => _entity.TopRight,
            BoundsHandleDirection.BottomRight => _entity.TopLeft,
            _ => Vector3.zero,
        };
    }

    private Vector3 GetToPoint(BoundsHandleDirection direction, Vector3 point)
    {
        return direction switch
        {
            BoundsHandleDirection.Right => point + GetRotatedPoint(new Vector3(0f, - _entity.Height / 2f, 0f)),
            BoundsHandleDirection.Left => point + GetRotatedPoint(new Vector3(0f, - _entity.Height / 2f, 0f)),
            BoundsHandleDirection.Top => point + GetRotatedPoint(new Vector3(_entity.Width / 2f, 0f, 0f)),
            BoundsHandleDirection.Bottom => point + GetRotatedPoint(new Vector3(_entity.Width / 2f, 0f, 0f)),
            _ => Vector3.zero,
        };
    }

    private Vector3 GetRotatedPoint(Vector3 point)
    {
        return Vector3Utils.RotatePointAroundPoint(point, Vector3.zero, _entity.transform.rotation);
    }
}