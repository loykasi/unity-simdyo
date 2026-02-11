using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PolygonTool : PanTool
{
    public override ToolType Type => ToolType.Polygon;

    private bool _onMouseHold;
    private bool _isDrawStraightLine;
    private List<Vector3> _points = new();
    private float _minimumDistance = 0.1f;

    public override void OnUpdate()
    {
        Zoom();
        HandlePanRightMouse();
        Create();
    }

    private void Create()
    {
        Vector3 mousePosition = GetMouseWorldPositon();

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _onMouseHold = true;
            
            Vector3 point = Vector3Utils.GetGridPosition(mousePosition);
            _points.Add(point);
            
            PolygonController.Instance.StartPreview();
            PolygonController.Instance.AddPoint(point);
            PolygonController.Instance.SetLastPoint(point);
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _onMouseHold = false;
            PolygonController.Instance.StopPreview();
            
            Vector3 point = Vector3Utils.GetGridPosition(mousePosition);
            if (_points.Count > 0 && Vector3.Distance(point, _points[^1]) > _minimumDistance)
            {
                _points.Add(point);
            }

            ObjectManager.Instance.AddPolygon(_points);
            _points.Clear();
        }

        if (Keyboard.current.shiftKey.wasPressedThisFrame)
        {
            _isDrawStraightLine = true;
        }

        if (Keyboard.current.shiftKey.wasReleasedThisFrame)
        {
            _isDrawStraightLine = false;
        }

        if (_onMouseHold)
        {
            bool shouldAddPoint = false;
            Vector3 current = Vector3Utils.GetGridPosition(mousePosition);
            Vector3 last = _points[^1];

            if (GridController.Instance.ShouldSnap)
            {
                Vector3 point = Vector3Utils.GetGridPosition(mousePosition);
                current = point;

                if (Vector3.Distance(current, last) > _minimumDistance)
                {
                    bool isDiagonal = (point.x != last.x) && (point.y != last.y);
                    float distSqr = (point - last).sqrMagnitude * (isDiagonal ? 0.5f : 0.2f);
                    float mouseToPoint = (point - mousePosition).sqrMagnitude;

                    if (mouseToPoint < distSqr)
                    {
                        current = point;
                        shouldAddPoint = true;
                    }   
                }
            }
            else
            {
                current = mousePosition;
                shouldAddPoint = true;
            }

            if (shouldAddPoint)
            {
                if (!_isDrawStraightLine && Vector3.Distance(current, last) > _minimumDistance)
                {
                    _points.Add(current);
                    shouldAddPoint = false;

                    PolygonController.Instance.AddPoint(current);
                }

                PolygonController.Instance.SetLastPoint(current);
            }
            
        }

        // if (Mouse.current.leftButton.wasPressedThisFrame)
        // {
        //     Vector3 point = Vector3Utils.GetGridPosition(mousePosition);
        //     _points.Add(point);
        //     Debug.Log($"Add {point}");
        // }

        // if (Keyboard.current.enterKey.wasPressedThisFrame)
        // {
        //     ObjectManager.Instance.AddPolygon(_points);
        //     _points.Clear();
        // }
    }
}