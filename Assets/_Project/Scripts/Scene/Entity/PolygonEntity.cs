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
            return BoundPosition + new Vector3(-Width / 2f, Height / 2f, 0f);
        }
    }

    public Vector3 TopRight
    {
        get
        {
            return BoundPosition + new Vector3(Width / 2f, Height / 2f, 0f);
        }
    }

    public Vector3 BottomRight
    {
        get
        {
            return BoundPosition + new Vector3(Width / 2f, -Height / 2f, 0f);
        }
    }

    public Vector3 BottomLeft
    {
        get
        {
            return BoundPosition + new Vector3(-Width / 2f, -Height / 2f, 0f);
        }
    }

    public Vector3 Left
    {
        get
        {
            return BoundPosition + new Vector3(-Width / 2f, 0f, 0f);
        }
    }

    public Vector3 Right
    {
        get
        {
            return BoundPosition + new Vector3(Width / 2f, 0f, 0f);
        }
    }

    public Vector3 Top
    {
        get
        {
            return BoundPosition + new Vector3(0f, Height / 2f, 0f);
        }
    }

    public Vector3 Bottom
    {
        get
        {
            return BoundPosition + new Vector3(0f, -Height / 2f, 0f);
        }
    }

    [SerializeField] private PolygonCollider2D _interactionArea;
    [SerializeField] private PolygonBorder _border;

    private List<Vector3> _vertices = new();
    private Vector2[] _collisionPoints;

    // Bounds
    public Vector3 BoundPosition => MeshFilter.sharedMesh.bounds.center + transform.position;
    public float Width => MeshFilter.sharedMesh.bounds.size.x;
    public float Height => MeshFilter.sharedMesh.bounds.size.y;

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
    }

    public void UpdateSize(Vector3 from, Vector3 to)
    {        
        Vector2 xNewRange = new(Mathf.Min(from.x, to.x), Mathf.Max(from.x, to.x));
        Vector2 yNewRange = new(Mathf.Min(from.y, to.y), Mathf.Max(from.y, to.y));

        float xScale = Mathf.Abs(from.x - to.x) / (Right.x - Left.x);
        float yScale = Mathf.Abs(from.y - to.y) / (Top.y - Bottom.y);

        if (xNewRange.y - xNewRange.x < 0.001
            || yNewRange.y - yNewRange.x < 0.001)
        {
            return;
        }

        transform.position = new Vector3
        (
            transform.position.x * xScale,
            transform.position.y * yScale,
            0f
        );

        xNewRange.x -= transform.position.x;
        xNewRange.y -= transform.position.x;
        yNewRange.x -= transform.position.y;
        yNewRange.y -= transform.position.y;

        for (int i = 0; i < _vertices.Count; i++)
        {
            float x = _vertices[i].x.MapRange(Left.x - transform.position.x, Right.x - transform.position.x, xNewRange.x, xNewRange.y);
            float y = _vertices[i].y.MapRange(Bottom.y - transform.position.y, Top.y - transform.position.y, yNewRange.x, yNewRange.y);

            _vertices[i] = new Vector3(x, y);
        }

        PolygonCollider2D collider = (PolygonCollider2D)Collider;
        for (int i = 0; i < _collisionPoints.Length; i++)
        {
            float x = _collisionPoints[i].x.MapRange(Left.x - transform.position.x, Right.x - transform.position.x, xNewRange.x, xNewRange.y);
            float y = _collisionPoints[i].y.MapRange(Bottom.y - transform.position.y, Top.y - transform.position.y, yNewRange.x, yNewRange.y);

            _collisionPoints[i] = new Vector2(x, y);
        }
        collider.points = _collisionPoints;
        _interactionArea.points = _collisionPoints;

        MeshFilter.sharedMesh.SetVertices(_vertices);
        MeshFilter.sharedMesh.RecalculateBounds();
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