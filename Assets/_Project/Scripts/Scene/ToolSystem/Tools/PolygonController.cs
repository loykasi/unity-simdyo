using System.Collections.Generic;
using UnityEngine;

public class PolygonController : Singleton<PolygonController>
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _baseWidth = 0.05f;
    private int _pointCount = 1;

    private void Update()
    {
        if (_lineRenderer.gameObject.activeInHierarchy)
        {
            Camera camera = EngineManager.Instance.EditorCamera;
            float width = camera.orthographicSize / 5f * _baseWidth;
            _lineRenderer.widthMultiplier = width;
        }
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