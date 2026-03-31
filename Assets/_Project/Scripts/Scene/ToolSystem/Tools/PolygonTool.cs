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

    [Header("Preview")]
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _baseWidth = 0.05f;
    private int _pointCount = 1;

    public override void OnUpdate()
    {
        Zoom();
        HandleSelection();
        HandleContextMenu();
        HandlePanRightMouse();
        Create();

        if (_lineRenderer.gameObject.activeInHierarchy)
        {
            Camera camera = EngineManager.Instance.EditorCamera;
            float width = camera.orthographicSize / 5f * _baseWidth;
            _lineRenderer.widthMultiplier = width;
        }
    }

    private void Create()
    {
        Vector3 mousePosition = GetMouseWorldPositon();

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _onMouseHold = true;
            
            Vector3 point = Vector3Utils.GetGridPosition(mousePosition);
            _points.Add(point);
            
            StartPreview();
            AddPoint(point);
            SetLastPoint(point);
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _onMouseHold = false;
            StopPreview();
            
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

                    AddPoint(current);
                }

                SetLastPoint(current);
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

    public void StartPreview()
    {
        _pointCount = 1;
        _lineRenderer.positionCount = _pointCount;

        _lineRenderer.gameObject.SetActive(true);
    }

    public void AddPoint(Vector3 point)
    {
        _lineRenderer.SetPosition(_pointCount - 1, point);
        _pointCount += 1;
        _lineRenderer.positionCount = _pointCount;
    }

    public void SetLastPoint(Vector3 point)
    {
        _lineRenderer.SetPosition(_pointCount - 1, point);
    }
    
    public void StopPreview()
    {
        _lineRenderer.gameObject.SetActive(false);
        _pointCount = 1;
    }
}