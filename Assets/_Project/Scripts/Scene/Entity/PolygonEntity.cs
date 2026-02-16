using System.Collections.Generic;
using Clipper2Lib;
using GameCore.Extensions;
using UnityEngine;

public class PolygonEntity : SceneEntity
{
    public override EntityType EntityType => EntityType.Polygon;

    public override Collider Collider => _collider;
    [SerializeField] private PolygonCollider _collider;

    public override Vector3 Position
    {
        get => transform.position;
        set
        {
            transform.position = value;
            UpdateBounds();
            OnUpdateProperty();
        }
    }

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

    public override Bounds Bounds => _bounds;
    private Bounds _bounds = new();

    public Vector3 TopLeft => Bounds.center + new Vector3(-Width / 2f, Height / 2f, 0f);
    public Vector3 TopRight => Bounds.center + new Vector3(Width / 2f, Height / 2f, 0f);
    public Vector3 BottomRight => Bounds.center + new Vector3(Width / 2f, -Height / 2f, 0f);
    public Vector3 BottomLeft => Bounds.center + new Vector3(-Width / 2f, -Height / 2f, 0f);
    public Vector3 Left => Bounds.center + new Vector3(-Width / 2f, 0f, 0f);
    public Vector3 Right => Bounds.center + new Vector3(Width / 2f, 0f, 0f);
    public Vector3 Top => Bounds.center + new Vector3(0f, Height / 2f, 0f);
    public Vector3 Bottom => Bounds.center + new Vector3(0f, -Height / 2f, 0f);
    public float Width => Bounds.size.x;
    public float Height => Bounds.size.y;

    [HideInInspector] public Vector2[] PolygonPoints;

    [SerializeField] private PolygonCollider2D _interactionArea;
    [SerializeField] private PolygonBorder _border;

    private List<Vector3> _vertices = new();
    private Vector3[] _temporaryPoints;

    private void UpdateBounds()
    {
        _temporaryPoints ??= new Vector3[_vertices.Count];
        for (int i = 0; i < _vertices.Count; i++)
        {
            _temporaryPoints[i] = _vertices[i];
        }
        transform.TransformPoints(_temporaryPoints);

        float maxX = Mathf.NegativeInfinity;
        float maxY = Mathf.NegativeInfinity;
        float minX = Mathf.Infinity;
        float minY = Mathf.Infinity;
        for (int i = 0; i < _temporaryPoints.Length; i++)
        {
            maxX = Mathf.Max(maxX, _temporaryPoints[i].x);
            maxY = Mathf.Max(maxY, _temporaryPoints[i].y);
            minX = Mathf.Min(minX, _temporaryPoints[i].x);
            minY = Mathf.Min(minY, _temporaryPoints[i].y);
        }

        _bounds.size = new Vector3(maxX - minX, maxY - minY);
        _bounds.center = new Vector3((maxX + minX) * 0.5f, (maxY + minY) * 0.5f);
    }

    public void SetPoints(Vector2[] points)
    {
        PolygonPoints = new Vector2[points.Length];
        points.CopyTo(PolygonPoints, 0);
        
        _collider.SetPoints(PolygonPoints);
        _interactionArea.points = PolygonPoints;

        _border.SetMesh(MeshFilter.sharedMesh);

        MeshFilter.sharedMesh.GetVertices(_vertices);
        UpdateBounds();
    }

    public void ApplyRotation()
    {
        // apply rotation to vertices and reset entity rotation
        if (Angle != 0)
        {
            for (int i = 0; i < _vertices.Count; i++)
            {
                _vertices[i] = Vector3Utils.RotatePointAroundPoint(_vertices[i], Vector3.zero, Rotation);
            }
            for (int i = 0; i < PolygonPoints.Length; i++)
            {
                PolygonPoints[i] = Vector3Utils.RotatePointAroundPoint(PolygonPoints[i], Vector3.zero, Rotation);
            }
            Angle = 0f;
            MeshFilter.sharedMesh.SetVertices(_vertices);
            MeshFilter.sharedMesh.RecalculateBounds();
            UpdateBounds();

            _collider.SetPoints(PolygonPoints);
            _interactionArea.points = PolygonPoints;
        }
    }

    public void UpdateSize(Vector3 from, Vector3 to)
    {
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

        for (int i = 0; i < PolygonPoints.Length; i++)
        {
            float x = PolygonPoints[i].x * xScale;
            float y = PolygonPoints[i].y * yScale;

            PolygonPoints[i] = new Vector2(x, y);
        }
        _collider.SetPoints(PolygonPoints);
        _interactionArea.points = PolygonPoints;

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

    public override SceneEntity CloneEntity()
    {
        PolygonEntity entity = ShapeGenerator.Instance.AddPolygon(Position, PolygonPoints);
        CopyPropertyTo(entity);
        ObjectManager.Instance.AddEntity(entity);

        return entity;
    }

    public override PathsD ToPaths()
    {
        int count = PolygonPoints.Length;
        double[] dpoints = new double[count * 2];

        for (int i = 0; i < count; i++)
        {
            dpoints[i * 2] = PolygonPoints[i].x + Position.x;
            dpoints[i * 2 + 1] = PolygonPoints[i].y + Position.y;
        }
        
        PathsD paths = new()
        {
            Clipper.MakePath(dpoints)
        };

        return paths;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (!IsColliderEnabled)
        {
            return;
        }

        Rigidbody2D rigidbody = collision.attachedRigidbody;
        Vector2 entityPosition = rigidbody.position;

        if (_collider.OverlapPoint(entityPosition))
        {
            Vector2 closestPoint = _collider.ClosestPoint(entityPosition);
            Vector2 direction = closestPoint - entityPosition;
            Vector2 normal = direction.normalized;

            Vector2 b = entityPosition - normal * 10000;
            float h = 10000 - Vector2.Distance(rigidbody.ClosestPoint(b) , b);

            float pushStrength = 20f;
            rigidbody.position += (direction.magnitude + h) * pushStrength * Time.fixedDeltaTime * normal;
        }
    }
}