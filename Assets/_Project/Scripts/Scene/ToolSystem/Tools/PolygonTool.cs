using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PolygonTool : PanTool
{
    public override ToolType Type => ToolType.Polygon;

    private bool _onMouseHold;
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
            _points.Add(Vector3Utils.GetGridPosition(mousePosition));
            _onMouseHold = true;
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            PolygonController.Instance.StopPreview();

            _onMouseHold = false;
            ObjectManager.Instance.AddPolygon(_points);
            _points.Clear();
        }

        if (_onMouseHold)
        {
            if (GridController.Instance.ShouldSnap)
            {
                // Vector3 last = _points[^1];
                // Vector3 point = Vector3Utils.GetGridPosition(mousePosition);

                // if (Vector3.Distance(point, _points[^1]) <= _minimumDistance)
                // {
                //     return;
                // }

                // bool isDiagonal = (point.x == last.x) || (point.y == last.y);
                // float distSqr = (point - last).sqrMagnitude * (isDiagonal ? 0.5f : 0.15f);
                // float mouseToPoint = (point - mousePosition).sqrMagnitude;

                // if (mouseToPoint < distSqr)
                // {
                //     _points.Add(point);
                //     PolygonController.Instance.Preview(_points);
                // }
            }
            else
            {
                Vector3 current = Vector3Utils.GetGridPosition(mousePosition);

                if (Vector3.Distance(current, _points[^1]) > _minimumDistance)
                {
                    _points.Add(current);
                    PolygonController.Instance.Preview(_points);
                }   
            }

            // Debug.Log($"Total: {_points.Count}");
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