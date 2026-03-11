using Clipper2Lib;
using Loykas.Scripting;
using UnityEngine;

public class CircleEntity : MeshEntity
{
    public override EntityType EntityType => EntityType.Circle;

    public override Collider Collider => _collider;
    [SerializeField] private CircleCollider _collider;

    public override Bounds Bounds => _bounds;
    private Bounds _bounds = new();

    public CircleBorder Border;

    public int TotalVert;
    public float Radius;

    [SerializeField] private CircleCollider2D _interactionCircle;

    private CircleState _circleState;
    private readonly int _radiusProperty = Shader.PropertyToID("_Radius");

    public void SetRadius(float radius, int totalVert = 10)
    {
        Radius = radius;
        TotalVert = totalVert;
        _collider.SetRadius(radius);

        float vertRadius = radius / Mathf.Cos(Mathf.PI / TotalVert);
        Vector3[] vertices = new Vector3[TotalVert];
        for (int i = 0; i < TotalVert; i++)
        {
            float x = vertRadius * Mathf.Sin(i * 2 * Mathf.PI / TotalVert);
            float y = vertRadius * Mathf.Cos(i * 2 * Mathf.PI / TotalVert);
            vertices[i] = new Vector3(x, y, 0f);
        }
        MeshFilter.mesh.vertices = vertices;
        Renderer.material.SetFloat(_radiusProperty, radius);

        _bounds.center = Position;
        _bounds.size = new Vector3(Radius * 2f, Radius * 2f);

        _interactionCircle.radius = radius;
        Border.SetRadius(Radius);
    }

    public void UpdateCircle(Vector3 from, Vector3 to)
    {
        Vector3 center = from;
        Radius = Vector3.Distance(from, to);

        transform.position = center;

        float vertRadius = Radius / Mathf.Cos(Mathf.PI / TotalVert);
        Vector3[] vertices = new Vector3[TotalVert];
        for (int i = 0; i < TotalVert; i++)
        {
            float x = vertRadius * Mathf.Sin(i * 2 * Mathf.PI / TotalVert);
            float y = vertRadius * Mathf.Cos(i * 2 * Mathf.PI / TotalVert);
            vertices[i] = new Vector3(x, y, 0f);
        }
        MeshFilter.mesh.vertices = vertices;
        MeshFilter.mesh.RecalculateBounds();
        Renderer.material.SetFloat(_radiusProperty, Radius);

        _collider.SetRadius(Radius);

        _bounds.center = Position;
        _bounds.size = new Vector3(Radius * 2f, Radius * 2f);

        _interactionCircle.radius = Radius;
        Border.SetRadius(Radius);
    }

    public override void Select()
    {
        Border.Enable();
        Border.SetRadius(Radius);
    }

    public override void Deselect()
    {
        Border.Disable();
    }

    public override void OnSceneStart()
    {
        base.OnSceneStart();
        _circleState = new()
        {
            Radius = Radius
        };
    }

    public override void OnSceneStop()
    {
        base.OnSceneStop();
        if (IsAddOnRuntime)
        {
            return;
        }
        SetRadius(_circleState.Radius);
    }

    public override SceneEntity CloneEntity()
    {
        CircleEntity entity = ShapeGenerator.Instance.AddCircle(Position, Radius);
        CopyPropertyTo(entity);
        ObjectManager.Instance.AddEntity(entity);

        return entity;
    }

    public override PathsD ToPaths()
    {
        float distancePerVertices = 0.05f;
        float angle = 2 * Mathf.Asin(distancePerVertices * 0.5f / Radius);

        int iterationCount = (int)(2 * Mathf.PI / angle);
        double[] dpoints = new double[iterationCount * 2];

        for (int i = 0; i < iterationCount; i++)
        {
            float x = Radius * Mathf.Sin(i * angle) + Position.x;
            float y = Radius * Mathf.Cos(i * angle) + Position.y;
            dpoints[i * 2] = x;
            dpoints[i * 2 + 1] = y;
        }
        
        PathsD paths = new()
        {
            Clipper.MakePath(dpoints)
        };

        return paths;
    }

    public override EntityData CreateSaveData()
    {
        return new CircleEntityData
        {
            Id = Id,
            Name = Name,
            Type = EntityType,
            Position = transform.position,
            Rotation = transform.rotation,
            ZDepth = ZDepth,
            Radius = Radius,
            ColliderEnabled = IsColliderEnabled,
            GravityEnabled = IsGravityEnabled,
            Layer = Layer,
            Color = CurrentColor,
            TextureSlotKey = TextureSlotKey
        };
    }
}