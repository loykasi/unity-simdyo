using System.Collections.Generic;
using Loykas.Scripting;
using UnityEngine;

public class PolygonController : Singleton<PolygonController>
{
    [SerializeField] private LineRenderer _lineRenderer;

    public void Preview(List<Vector3> points)
    {
        _lineRenderer.gameObject.SetActive(true);
        _lineRenderer.positionCount = points.Count;
        _lineRenderer.SetPositions(points.ToArray());
    }

    public void StopPreview()
    {
        _lineRenderer.gameObject.SetActive(false);
    }
}