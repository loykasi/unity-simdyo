using System.Collections.Generic;
using GameCore.Extensions;
using Loykas.Scripting;
using UnityEngine;

public class PolygonEntity : SceneEntity
{
    public Vector3 TopLeft
    {
        get
        {
            return Bounds.center + new Vector3(-Width / 2f, Height / 2f, 0f);
        }
    }

    public Vector3 TopRight
    {
        get
        {
            return Bounds.center + new Vector3(Width / 2f, Height / 2f, 0f);
        }
    }

    public Vector3 BottomRight
    {
        get
        {
            return Bounds.center + new Vector3(Width / 2f, -Height / 2f, 0f);
        }
    }

    public Vector3 BottomLeft
    {
        get
        {
            return Bounds.center + new Vector3(-Width / 2f, -Height / 2f, 0f);
        }
    }

    public Vector3 Left
    {
        get
        {
            return Bounds.center + new Vector3(-Width / 2f, 0f, 0f);
        }
    }

    public Vector3 Right
    {
        get
        {
            return Bounds.center + new Vector3(Width / 2f, 0f, 0f);
        }
    }

    public Vector3 Top
    {
        get
        {
            return Bounds.center + new Vector3(0f, Height / 2f, 0f);
        }
    }

    public Vector3 Bottom
    {
        get
        {
            return Bounds.center + new Vector3(0f, -Height / 2f, 0f);
        }
    }

    public override Bounds Bounds => _bounds;
    private Bounds _bounds = new();

    public override Quaternion Rotation
    {
        get => transform.rotation;
        set
        {
            transform.rotation = value;
            UpdateBounds();
            OnUpdateProperty();
        }
    }

    [SerializeField] private PolygonCollider2D _interactionArea;
    [SerializeField] private PolygonBorder _border;

    private List<Vector3> _vertices = new();
    private Vector2[] _collisionPoints;

    // Bounds
    
    public float Width => Bounds.size.x;
    public float Height => Bounds.size.y;

    private void UpdateBounds()
    {
        Vector3[] points = new Vector3[_vertices.Count];
        for (int i = 0; i < _vertices.Count; i++)
        {
            points[i] = _vertices[i];
        }

        transform.TransformPoints(points);

        float maxX = Mathf.NegativeInfinity;
        float maxY = Mathf.NegativeInfinity;
        float minX = Mathf.Infinity;
        float minY = Mathf.Infinity;
        for (int i = 0; i < points.Length; i++)
        {
            maxX = Mathf.Max(maxX, points[i].x);
            maxY = Mathf.Max(maxY, points[i].y);
            minX = Mathf.Min(minX, points[i].x);
            minY = Mathf.Min(minY, points[i].y);
        }

        _bounds.size = new Vector3(maxX - minX, maxY - minY);
        _bounds.center = new Vector3((maxX + minX) * 0.5f, (maxY + minY) * 0.5f);
    }

    public void SetVertices(List<Vector3> points)
    {
        PolygonCollider2D collider = (PolygonCollider2D)Collider;

        Vector2[] vertices = new Vector2[points.Count];
        for (int i = points.Count - 1; i >= 0; i--)
        {
            vertices[i] = (Vector2)points[i];
        }

        _collisionPoints = new Vector2[points.Count];
        for (int i = 0; i < points.Count; i++)
        {
            _collisionPoints[i] = points[i];
        }
        collider.points = _collisionPoints;
        _interactionArea.points = _collisionPoints;

        _border.SetMesh(MeshFilter.sharedMesh, points);

        MeshFilter.sharedMesh.GetVertices(_vertices);

        UpdateBounds();
    }

    public void UpdateSize(Vector3 from, Vector3 to)
    {
        // apply rotation to vertices and reset entity rotation
        if (Angle != 0)
        {
            for (int i = 0; i < _vertices.Count; i++)
            {
                _vertices[i] = Vector3Utils.RotatePointAroundPoint(_vertices[i], Vector3.zero, Rotation);
            }
            for (int i = 0; i < _collisionPoints.Length; i++)
            {
                _collisionPoints[i] = Vector3Utils.RotatePointAroundPoint(_collisionPoints[i], Vector3.zero, Rotation);
            }
            Angle = 0f;
            MeshFilter.sharedMesh.SetVertices(_vertices);
            MeshFilter.sharedMesh.RecalculateBounds();
            UpdateBounds();

            PolygonCollider2D polygonCollider = (PolygonCollider2D)Collider;
            polygonCollider.points = _collisionPoints;
            _interactionArea.points = _collisionPoints;
        }

        Vector2 xOldRange = new(Left.x, Right.x);
        Vector2 yOldRange = new(Bottom.y, Top.y);
        Vector2 xNewRange = new(Mathf.Min(from.x, to.x), Mathf.Max(from.x, to.x));
        Vector2 yNewRange = new(Mathf.Min(from.y, to.y), Mathf.Max(from.y, to.y));

        float xScale = Mathf.Abs(from.x - to.x) / (Right.x - Left.x);
        float yScale = Mathf.Abs(from.y - to.y) / (Top.y - Bottom.y);

        if (xNewRange.y - xNewRange.x < 0.001
            || yNewRange.y - yNewRange.x < 0.001)
        {
            return;
        }

        Position = new Vector3
        (
            Position.x.MapRange(xOldRange.x, xOldRange.y, xNewRange.x, xNewRange.y),
            Position.y.MapRange(yOldRange.x, yOldRange.y, yNewRange.x, yNewRange.y),
            0f
        );

        for (int i = 0; i < _vertices.Count; i++)
        {
            float x = _vertices[i].x * xScale;
            float y = _vertices[i].y * yScale;

            _vertices[i] = new Vector3(x, y);
        }

        PolygonCollider2D collider = (PolygonCollider2D)Collider;
        for (int i = 0; i < _collisionPoints.Length; i++)
        {
            float x = _collisionPoints[i].x * xScale;
            float y = _collisionPoints[i].y * yScale;

            _collisionPoints[i] = new Vector2(x, y);
        }
        collider.points = _collisionPoints;
        _interactionArea.points = _collisionPoints;

        MeshFilter.sharedMesh.SetVertices(_vertices);
        MeshFilter.sharedMesh.RecalculateBounds();
        UpdateBounds();
    }

    public override void Select()
    {
        _border.Enable(CurrentColor);
    }

    public override void Deselect()
    {
        _border.Disable();
    }

}