using Clipper2Lib;
using Loykas.Scripting;
using UnityEngine;

public class CircleEntity : MeshEntity
{
    public override EntityType EntityType => EntityType.Circle;

    public float Radius;

    public override Collider Collider => _collider;
    [SerializeField] private CircleCollider _collider;

    public override Bounds Bounds => _bounds;
    private Bounds _bounds = new();

    [SerializeField] private CircleBorder _border;
    [SerializeField] private CircleCollider2D _interactionCircle;

    private CircleState _circleState;
    private readonly int _radiusProperty = Shader.PropertyToID("_Radius");

    public void SetMesh(MeshWrapper meshWrapper, Material material, float radius)
    {
        Radius = radius;
        Mesh = meshWrapper;
        Material = Instantiate(material);

        Renderer.material = Material;
        meshWrapper.AssignTo(MeshFilter);

        Material.SetFloat(_radiusProperty, Radius);
        _interactionCircle.radius = Radius;
        _border.SetMesh(meshWrapper);
        _border.SetRadius(Radius);
        _collider.SetRadius(Radius);

        _bounds.center = Position;
        _bounds.size = new Vector3(Radius * 2f, Radius * 2f);
    }

    public void SetRadius(float radius)
    {
        Radius = radius;

        int length = ShapeGenerator.TotalVert;
        float angleStep = 2 * Mathf.PI / length;
        float vertRadius = Radius / Mathf.Cos(Mathf.PI / length);
        for (int i = 0; i < length; i++)
        {
            float angle = i * angleStep;
            Mesh.Vertices[i] = new Vector3
            (
                vertRadius * Mathf.Sin(angle),
                vertRadius * Mathf.Cos(angle),
                0f
            );
        }
        Mesh.Update();

        Material.SetFloat(_radiusProperty, Radius);
        _interactionCircle.radius = Radius;
        _border.SetRadius(Radius);
        _collider.SetRadius(Radius);

        _bounds.center = Position;
        _bounds.size = new Vector3(Radius * 2f, Radius * 2f);
    }

    public void UpdateCircle(Vector3 from, Vector3 to)
    {
        Vector3 center = from;
        Radius = Vector3.Distance(from, to);

        transform.position = center;

        SetRadius(Radius);
    }

    public override void Select()
    {
        _border.Enable();
        _border.SetRadius(Radius);
    }

    public override void Deselect()
    {
        _border.Disable();
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
        CircleEntity entity = ShapeGenerator.Instance.Clone(this);
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