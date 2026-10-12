using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PolygonTool : BaseTool
{
    public override ToolType Type => ToolType.Polygon;

    private bool _onMouseHold;
    private bool _isDrawStraightLine;
    private readonly List<Vector3> _points = new();
    private float _minimumDistance = 0.1f;

    public override void Disable()
    {
        if (_onMouseHold)
        {
            ShapePreview.Instance.StopPolygonPreview();
            _onMouseHold = false;
            _isDrawStraightLine = false;
            _points.Clear();
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (Keyboard.current.shiftKey.wasPressedThisFrame)
        {
            _isDrawStraightLine = true;
        }

        if (Keyboard.current.shiftKey.wasReleasedThisFrame)
        {
            _isDrawStraightLine = false;
        }
    }

    public override bool OnPointerLeftDown(Vector2 position)
    {
        _onMouseHold = true;
        Vector3 mouseWorldPosition = Utils.ToWorldPositon(InputManager.Instance.MousePosition);
        Vector3 point = Vector3Utils.GetGridPosition(mouseWorldPosition);
        _points.Add(point);
        
        ShapePreview.Instance.StartPolygonPreview();
        ShapePreview.Instance.AddPolygonPoint(point);
        ShapePreview.Instance.SetLastPoint(point);

        return true;
    }

    public override void OnPointerLeftUp(Vector2 position)
    {
        if (_onMouseHold)
        {
            _onMouseHold = false;
            ShapePreview.Instance.StopPolygonPreview();
            
            Vector3 mouseWorldPosition = Utils.ToWorldPositon(InputManager.Instance.MousePosition);
            Vector3 point = Vector3Utils.GetGridPosition(mouseWorldPosition);
            if (_points.Count > 0 && Vector3.Distance(point, _points[^1]) > _minimumDistance)
            {
                _points.Add(point);
            }

            ObjectManager.Instance.AddPolygon(_points);
            _points.Clear();
        }
    }

    public override void OnPointerMove(Vector2 position)
    {
        if (_onMouseHold)
        {
            Vector3 mousePosition = Utils.ToWorldPositon(InputManager.Instance.MousePosition);

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

                    ShapePreview.Instance.AddPolygonPoint(current);
                }

                ShapePreview.Instance.SetLastPoint(current);
            }   
        }
    }
}