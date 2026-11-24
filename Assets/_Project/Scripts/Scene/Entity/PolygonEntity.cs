using System.Collections.Generic;
using Clipper2Lib;
using GameCore.Extensions;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PolygonEntity : MeshEntity
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
            OnPropertyUpdated?.Invoke();
        }
    }

    public override Quaternion Rotation
    {
        get => transform.rotation;
        set
        {
            transform.rotation = value;
            UpdateBounds();
            OnPropertyUpdated?.Invoke();
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

    private Vector3[] _temporaryPoints;
    private int _sdBakedSize = 128;
    private Texture2D _pointTexture;
    private RenderTexture _rt;
    [SerializeField] private Material _bakeMaterial;

    private static readonly int SDTextureProperty = Shader.PropertyToID("_SDTexture");
    private static readonly int SizeProperty = Shader.PropertyToID("_Size");
    private static readonly int CenterProperty = Shader.PropertyToID("_Center");
    private static readonly int PointTextureProperty = Shader.PropertyToID("_PointTexture");
    private static readonly int PointCountProperty = Shader.PropertyToID("_PointCount");
    private static readonly int SDRangeProperty = Shader.PropertyToID("_SDRange");

    protected override void Awake()
    {
        base.Awake();
        _rt = new RenderTexture(_sdBakedSize, _sdBakedSize, 0, RenderTextureFormat.R8, RenderTextureReadWrite.Linear);
    }

    public void SetMesh(MeshWrapper meshWrapper, Material material, Vector2[] points)
    {
        Mesh = meshWrapper;
        Material = Instantiate(material);

        Mesh.AssignTo(MeshFilter);
        Renderer.sharedMaterial = Material;

        PolygonPoints = points;
        _collider.SetPoints(PolygonPoints);
        _interactionArea.points = PolygonPoints;
        UpdateBounds();

        BakeSDF(points);
    }

    private void BakeSDF(Vector2[] points)
    {
        float range = 1f;

        if (!_pointTexture)
        {
            _pointTexture = new(points.Length, 1, TextureFormat.RGFloat, false, true)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };
        }
        NativeArray<Vector2> pointData = _pointTexture.GetRawTextureData<Vector2>();

        Vector2 min = points[0];
        Vector2 max = points[0];
        for (int i = 0; i < points.Length; i++)
        {
            Vector2 point = points[i];
            min = Vector2.Min(min, point);
            max = Vector2.Max(max, point);
            pointData[i] = point;
        }
        _pointTexture.Apply();
        float size = Mathf.Max(max.x - min.x, max.y - min.y) + 2f * range;
        Vector2 center = (min + max) * 0.5f;

        _bakeMaterial.SetVector(CenterProperty, center);
        _bakeMaterial.SetFloat(SizeProperty, size);
        _bakeMaterial.SetTexture(PointTextureProperty, _pointTexture);
        _bakeMaterial.SetFloat(PointCountProperty, points.Length);
        _bakeMaterial.SetFloat(SDRangeProperty, range);

        Graphics.Blit(null, _rt, _bakeMaterial);

        Material.SetTexture(SDTextureProperty, _rt);
        Material.SetFloat(SizeProperty, size);
        Material.SetVector(CenterProperty, center);
        Material.SetFloat(SDRangeProperty, range);

        // keep cpu bake code, to test on webgl, low end devices
        
        // if (!_sdTexture)
        // {
        //     _sdTexture = new(_sdBakedSize, _sdBakedSize, TextureFormat.RHalf, false, true)
        //     {
        //         filterMode = FilterMode.Bilinear,
        //         wrapMode = TextureWrapMode.Clamp
        //     };    
        // }
        
        // NativeArray<ushort> data = _sdTexture.GetRawTextureData<ushort>();
        // float texelSize = size / _sdBakedSize;
        // for (int i = 0; i < _sdBakedSize * _sdBakedSize; i++)
        // {
        //     Vector2Int texel = new(i % _sdBakedSize, i / _sdBakedSize);
        //     Vector2 point = center + new Vector2
        //     (
        //         (texel.x + 0.5f) * texelSize - size * 0.5f,
        //         (texel.y + 0.5f) * texelSize - size * 0.5f
        //     );

        //     float d = SDPolygon(points, point);
        //     // float encoded = Mathf.Clamp01(d / (2f * range) + 0.5f);
        //     // data[i] = (byte)(encoded * 255f);
        //     data[i] = Mathf.FloatToHalf(d);
        // }
        // _sdTexture.Apply(false, false);

        // Material.SetTexture(SDTextureProperty, _sdTexture);
        // Material.SetFloat(SizeProperty, size);
        // Material.SetVector(CenterProperty, center);
    }

    // private float SDPolygon(Vector2[] points, Vector2 position)
    // {
    //     float d = Vector2.Dot(position - points[0], position - points[0]);
    //     float s = 1.0f;
    //     for(int i = 0, j = points.Length - 1; i < points.Length; j = i, i++)
    //     {
    //         Vector2 e = points[j] - points[i];
    //         Vector2 w = position - points[i];
    //         Vector2 b = w - e * Mathf.Clamp(Vector2.Dot(w, e) / Vector2.Dot(e, e), 0.0f, 1.0f);
    //         d = Mathf.Min(d, Vector2.Dot(b, b));
            
    //         bool x = position.y >= points[i].y;
    //         bool y = position.y < points[j].y;
    //         bool z = e.x*w.y>e.y*w.x;
    //         if((x && y && z) || (!x && !y && !z))
    //             s *= -1.0f;
    //     }
    //     return s * Mathf.Sqrt(d);
    // }

    private void UpdateBounds()
    {
        _temporaryPoints ??= new Vector3[Mesh.VertexCount];
        for (int i = 0; i < Mesh.VertexCount; i++)
        {
            _temporaryPoints[i] = Mesh.Vertices[i];
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

    public void ApplyRotation()
    {
        // apply rotation to vertices and reset entity rotation
        if (Angle != 0)
        {
            for (int i = 0; i < Mesh.VertexCount; i++)
            {
                Mesh.Vertices[i] = Vector3Utils.RotatePointAroundPoint(Mesh.Vertices[i], Vector3.zero, Rotation);
            }
            for (int i = 0; i < PolygonPoints.Length; i++)
            {
                PolygonPoints[i] = Vector3Utils.RotatePointAroundPoint(PolygonPoints[i], Vector3.zero, Rotation);
            }
            Angle = 0f;
            Mesh.Update();
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

        for (int i = 0; i < Mesh.VertexCount; i++)
        {
            float x = Mesh.Vertices[i].x * xScale;
            float y = Mesh.Vertices[i].y * yScale;

            Mesh.Vertices[i] = new Vector3(x, y);
        }

        for (int i = 0; i < PolygonPoints.Length; i++)
        {
            float x = PolygonPoints[i].x * xScale;
            float y = PolygonPoints[i].y * yScale;

            PolygonPoints[i] = new Vector2(x, y);
        }
        _collider.SetPoints(PolygonPoints);
        _interactionArea.points = PolygonPoints;

        Mesh.Update();
        UpdateBounds();
        BakeSDF(PolygonPoints);
    }

    public override SceneEntity CloneEntity()
    {
        PolygonEntity entity = ShapeGenerator.Clone(this);
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
        if (rigidbody.bodyType == RigidbodyType2D.Static)
        {
            return;
        }

        if (collision.TryGetComponent(out SceneEntity entity) && entity.EntityType == EntityType.Polygon)
        {
            return;
        }
        
        Vector2 entityPosition = rigidbody.position;
        if (_collider.OverlapPoint(entityPosition))
        {
            Vector2 closestPoint = _collider.ClosestPoint(entityPosition);
            Vector2 direction = closestPoint - entityPosition;
            Vector2 normal = direction.normalized;

            Vector2 b = entityPosition - normal * 10000;
            float h = 10000 - Vector2.Distance(rigidbody.ClosestPoint(b) , b);

            float pushStrength = 5f;
            // rigidbody.position += (direction.magnitude + h) * pushStrength * Time.fixedDeltaTime * normal;
            rigidbody.linearVelocity += pushStrength * normal;
        }
    }

    public override EntityData CreateSaveData()
    {
        return new PolygonEntityData
        {
            Id = Id,
            Name = Name,
            Type = EntityType,
            Position = transform.position,
            Rotation = transform.rotation,
            ZDepth = ZDepth,
            PolygonPoints = PolygonPoints,
            ColliderEnabled = IsColliderEnabled,
            GravityEnabled = IsGravityEnabled,
            Layer = Layer,
            Color = CurrentColor,
            TextureSlotKey = TextureSlotKey
        };
    }
}