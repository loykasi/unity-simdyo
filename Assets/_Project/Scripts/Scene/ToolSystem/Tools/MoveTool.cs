using UnityEngine;
using UnityEngine.InputSystem;

public class MoveTool : PanTool
{
    public override ToolType Type => ToolType.Move;

    private bool _onMovingObject = false;
    private Vector3 _mouseStartPosition;
    private Vector3 _entityStartPosition;
    private Bounds _bounds;
    private float[] _asixPoints = new float[3];

    public override void OnUpdate()
    {
        base.Zoom();
        base.HandlePanRightMouse();
        base.HandleSelection();
        base.HandleContextMenu();
        Move();
    }

    public void Move()
    {
        Vector3 mousePosition = GetMouseWorldPositon();

        if (Mouse.current.leftButton.wasPressedThisFrame && !ScreenInteractionUtils.IsOverUI())
        {
            var selected = ObjectManager.Instance.SelectedObject;
            if (selected == null)
            {
                return;
            }

            ObjectManager.Instance.TryGetSceneEntity
            (
                EngineManager.Instance.EditorCamera,
                Mouse.current.position.ReadValue(),
                out SceneEntity onMouseEntity
            );

            if (onMouseEntity == selected)
            {
                _onMovingObject = true;
                _mouseStartPosition = mousePosition;
                _entityStartPosition = selected.Position;
                _bounds = selected.Bounds;
            }
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _onMovingObject = false;

            var selected = ObjectManager.Instance.SelectedObject;
            if (selected is TracerEntity tracerEntity)
            {
                tracerEntity.AutoAttachToMeshEntity();
            }

            Physics2D.SyncTransforms();
        }

        if (_onMovingObject)
        {
            ApplyMovementWithSnapping(mousePosition);
        }
    }

    private void ApplyMovementWithSnapping(Vector3 mousePosition)
    {
        var selected = ObjectManager.Instance.SelectedObject;

        Vector3 center = _bounds.center;
        Vector3 extents = _bounds.extents;

        Vector3 mouseOffset = mousePosition - _mouseStartPosition;

        float snappedX = GetSnappedOffset(mouseOffset, center, extents.x, isYAsis: false);
        float snappedY = GetSnappedOffset(mouseOffset, center, extents.y, isYAsis: true);

        selected.Position = _entityStartPosition + new Vector3(snappedX, snappedY, 0);
    }

    private float GetSnappedOffset(Vector3 mouseOffset, Vector3 center, float extent, bool isYAsis)
    {
        _asixPoints[0] = 0;
        _asixPoints[1] = extent;
        _asixPoints[2] = - extent;
        
        float bestOffset = 0;
        float minSqrLen = float.MaxValue;

        foreach (float axisPoint in _asixPoints)
        {
            Vector3 point = center + (isYAsis ? new Vector3(0, axisPoint, 0) : new Vector3(axisPoint, 0, 0));
            CalculateSnapping(point, mouseOffset, isYAsis: isYAsis, out float offset, out float sqrLen);

            if (sqrLen < minSqrLen)
            {
                minSqrLen = sqrLen;
                bestOffset = offset;
            }
        }

        return bestOffset;
    }

    private void CalculateSnapping(Vector3 point, Vector3 mouseOffset, bool isYAsis, out float moveOffset, out float minSqrLen)
    {
        Vector3 movePoint = point + mouseOffset;
        Vector3 gridPos = Vector3Utils.GetGridPosition(movePoint);
        
        moveOffset = isYAsis ? (gridPos.y - point.y) : (gridPos.x - point.x);
        minSqrLen = Mathf.Abs(isYAsis ? (gridPos.y - movePoint.y) : (gridPos.x - movePoint.x));
    }
}