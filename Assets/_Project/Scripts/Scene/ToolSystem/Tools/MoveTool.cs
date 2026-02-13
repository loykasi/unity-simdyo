using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveTool : PanTool
{
    public override ToolType Type => ToolType.Move;

    private bool _onMovingObject = false;
    // private Vector3 _offsetFromMouse;
    private Vector3 _mouseStartPosition;
    private Vector3 _entityStartPosition;
    private Bounds _bounds;
    List<Vector3> points = new();

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
            if (onMouseEntity != selected)
            {
                return;
            }

            // _offsetFromMouse = selected.transform.position - Vector3Utils.GetGridPosition(mousePosition);
            _onMovingObject = true;
            _mouseStartPosition = mousePosition;
            _entityStartPosition = selected.Position;
            _bounds = selected.Bounds;
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _onMovingObject = false;
            Physics2D.SyncTransforms();
        }

        if (_onMovingObject)
        {
            var selected = ObjectManager.Instance.SelectedObject;

            float halfWidth = _bounds.size.x / 2f;
            float halfHeight = _bounds.size.y / 2f;

            Vector3 center = _bounds.center;
            Vector3 top = _bounds.center + new Vector3(0f, halfHeight, 0f);
            Vector3 bottom = _bounds.center + new Vector3(0f, - halfHeight, 0f);
            Vector3 left = _bounds.center + new Vector3(- halfWidth, 0f, 0f);
            Vector3 right = _bounds.center + new Vector3(halfWidth, 0f, 0f);

            Vector3 mouseOffset = mousePosition - _mouseStartPosition;
            float minSqrLen = 0;

            points.Clear();
            points.Add(center);
            points.Add(top);
            points.Add(bottom);

            Vector3 moveOffset = Vector3.zero;

            CalculateOffSetAndDistanceY(points[0], mouseOffset, out moveOffset.y, out minSqrLen);
            for (int i = 1; i < points.Count; i++)
            {
                CalculateOffSetAndDistanceY(points[i], mouseOffset, out float offset, out float sqrLen);
                if (sqrLen < minSqrLen)
                {
                    minSqrLen = sqrLen;
                    moveOffset.y = offset;
                }
            }

            points.Clear();
            points.Add(center);
            points.Add(left);
            points.Add(right);

            CalculateOffSetAndDistanceX(points[0], mouseOffset, out moveOffset.x, out minSqrLen);
            for (int i = 1; i < points.Count; i++)
            {
                CalculateOffSetAndDistanceX(points[i], mouseOffset, out float offset, out float sqrLen);
                if (sqrLen < minSqrLen)
                {
                    minSqrLen = sqrLen;
                    moveOffset.x = offset;
                }
            }

            selected.Position = _entityStartPosition + moveOffset;

            // DebugPoint(center + mouseOffset, Color.black);
            // DebugPoint(top + mouseOffset, Color.black);
            // DebugPoint(bottom + mouseOffset, Color.black);
            // DebugPoint(Vector3Utils.GetGridPosition(center + mouseOffset) + Vector3.left * 0.2f, Color.red);
            // DebugPoint(Vector3Utils.GetGridPosition(top + mouseOffset), Color.red);
            // DebugPoint(Vector3Utils.GetGridPosition(bottom + mouseOffset) + Vector3.right * 0.2f, Color.red);
        }
    }

    private void CalculateOffSetAndDistanceY(Vector3 point, Vector3 mouseOffset, out float moveOffset, out float minSqrLen)
    {
        Vector3 movePoint = point + mouseOffset;
        moveOffset = (Vector3Utils.GetGridPosition(movePoint) - point).y;
        minSqrLen = (Vector3Utils.GetGridPosition(movePoint) - movePoint).y;
        minSqrLen = Mathf.Abs(minSqrLen);
    }

    private void CalculateOffSetAndDistanceX(Vector3 point, Vector3 mouseOffset, out float moveOffset, out float minSqrLen)
    {
        Vector3 movePoint = point + mouseOffset;
        moveOffset = (Vector3Utils.GetGridPosition(movePoint) - point).x;
        minSqrLen = (Vector3Utils.GetGridPosition(movePoint) - movePoint).x;
        minSqrLen = Mathf.Abs(minSqrLen);
    }

    private void DebugPoint(Vector3 point, Color color)
    {
        Debug.DrawRay(point, Vector3.up * 0.2f, color);
        Debug.DrawRay(point, Vector3.down * 0.2f, color);
        Debug.DrawRay(point, Vector3.right * 0.2f, color);
        Debug.DrawRay(point, Vector3.left * 0.2f, color);
    }
}