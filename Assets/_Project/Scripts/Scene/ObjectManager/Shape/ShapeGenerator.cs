using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator : Singleton<ShapeGenerator>
{
    public const int TotalVert = 10;

    [SerializeField] private BoxEntity _boxEntityPrefab;
    [SerializeField] private CircleEntity _circleEntityPrefab;
    [SerializeField] private PolygonEntity _polygonEntityPrefab;
    [SerializeField] private Material _material;
    [SerializeField] private Material _circleMaterial;


    // ==== ADD BOX ====

    public BoxEntity AddBox(Vector3 from, Vector3 to)
    {
        if (from == to)
        {
            return null;
        }

        Vector3 position = (from + to) / 2f;
        float width = Mathf.Abs(from.x - to.x);
        float height = Mathf.Abs(from.y - to.y);

        return AddBox(position, width, height);
    }

    public BoxEntity AddBox(Vector3 position, float width, float height)
    {
        if (width == 0 || height == 0)
        {
            return null;
        }

        BoxEntity box = Instantiate(_boxEntityPrefab);
        MeshWrapper mesh = BoxMeshGenerator.Generate(new Vector2(width, height));
        
        box.name = "Box";
        box.transform.position = position;
        box.SetMesh(mesh, _material, width, height);
        box.CurrentColor = GetRandomColor();

        Physics2D.SyncTransforms();
        return box;
    }

    public BoxEntity Clone(BoxEntity entity)
    {
        BoxEntity box = Instantiate(_boxEntityPrefab);
        MeshWrapper mesh = new(entity.Mesh);

        box.name = "Box";
        box.transform.position = entity.Position;
        box.SetMesh(mesh, _material, entity.Width, entity.Height);

        return box;
    }

    // ==== CIRCLE ====

    public CircleEntity AddCircle(Vector3 from, Vector3 to)
    {
        if (from == to)
        {
            return null;
        }

        float radius = Vector3.Distance(from, to);

        return AddCircle(from, radius);
    }

    public CircleEntity AddCircle(Vector3 position, float radius)
    {
        if (radius == 0)
        {
            return null;
        }

        CircleEntity sceneEntity = Instantiate(_circleEntityPrefab);
        MeshWrapper mesh = CircleMeshGenerator.Generate(radius);

        sceneEntity.name = "Circle";
        sceneEntity.transform.position = position;
        sceneEntity.SetMesh(mesh, _circleMaterial, radius);
        sceneEntity.CurrentColor = GetRandomColor();

        Physics2D.SyncTransforms();

        return sceneEntity;
    }

    public CircleEntity Clone(CircleEntity entity)
    {
        CircleEntity circle = Instantiate(_circleEntityPrefab);
        MeshWrapper mesh = new(entity.Mesh);

        circle.name = "Circle";
        circle.transform.position = entity.Position;
        circle.SetMesh(mesh, _material, entity.Radius);

        return circle;
    }

    // ==== ADD POLYGON ====

    public PolygonEntity AddPolygon(List<Vector3> points)
    {
        if (points.Count <= 2) return null;

        Vector3 center = Vector3.zero;
        foreach (var point in points)
        {
            center += point;
        }
        center /= points.Count;

        Vector2[] polygonPoints = new Vector2[points.Count];
        for (int i = 0; i < points.Count; i++)
        {
            polygonPoints[i] = points[i] - center;
        }

        return AddPolygon(center, polygonPoints);
    }

    public PolygonEntity AddPolygon(Vector3 position, Vector2[] points)
    {
        if (points.Length <= 2) return null;

        MeshWrapper meshWrapper = PolygonMeshGenerator.Generate(points);

        PolygonEntity sceneEntity = Instantiate(_polygonEntityPrefab);
        sceneEntity.name = "Polygon";
        sceneEntity.transform.position = position;
        sceneEntity.SetMesh(meshWrapper, _material, points);
        sceneEntity.CurrentColor = GetRandomColor();

        Physics2D.SyncTransforms();
        
        return sceneEntity;
    }

    public PolygonEntity Clone(PolygonEntity entity)
    {
        PolygonEntity sceneEntity = Instantiate(_polygonEntityPrefab);
        MeshWrapper mesh = new(entity.Mesh);
        
        int length = entity.PolygonPoints.Length;
        Vector2[] points = new Vector2[length];
        System.Array.Copy(entity.PolygonPoints, points, length);

        sceneEntity.name = "Polygon";
        sceneEntity.transform.position = entity.Position;
        sceneEntity.SetMesh(mesh, _material, points);

        Physics2D.SyncTransforms();
        
        return sceneEntity;
    }

    private ColorHSV GetRandomColor()
    {
        return new ColorHSV
        (
            (float)Random.Range(0, 361) / 360,
            (float)Random.Range(0, 101) / 100,
            (float)Random.Range(0, 101) / 100,
            1f
        );
    }

    [ContextMenu("Add Circle")]
    private void AddCircle()
    {
        GameObject shape = new()
        {
            name = "Circle"
        };

        MeshFilter meshFilter = shape.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = shape.AddComponent<MeshRenderer>();

        Vector3 center = Vector3.zero;
        float radius = 1f;

        shape.transform.position = center;

        float vertRadius = radius / Mathf.Cos(Mathf.PI / TotalVert);
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < TotalVert; i++)
        {
            float x = vertRadius * Mathf.Sin(i * 2 * Mathf.PI / TotalVert);
            float y = vertRadius * Mathf.Cos(i * 2 * Mathf.PI / TotalVert);
            points.Add(new Vector3(x, y, 0f));
        }

        List<int> trianglesList = new List<int>();
        for (int i = 0; i < TotalVert - 2; i++)
        {
            trianglesList.Add(0);
            trianglesList.Add(i + 1);
            trianglesList.Add(i + 2);
        }
        int[] triangles = trianglesList.ToArray();

        Mesh mesh = new()
        {
            name = "Circle"
        };
        mesh.SetVertices(points);
        mesh.triangles = triangles;

        meshFilter.sharedMesh = mesh;
        meshRenderer.sharedMaterial = _circleMaterial;
    }

    [ContextMenu("Add Box")]
    private void AddBox()
    {
        Vector3 from = Vector3.zero;
        Vector3 to = new(1f, 1f);

        GameObject shape = new()
        {
            name = "Box"
        };
        MeshFilter meshFilter = shape.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = shape.AddComponent<MeshRenderer>();

        Vector3 center = (from + to) / 2f;
        float halfWidth = Mathf.Abs(from.x - to.x) / 2f;
        float halfHeight = Mathf.Abs(from.y - to.y) / 2f;

        shape.transform.position = center;

        List<Vector3> points = new List<Vector3>()
        {
            new Vector3(halfWidth, halfHeight),
            new Vector3(- halfWidth, halfHeight),
            new Vector3(- halfWidth, - halfHeight),
            new Vector3(halfWidth, - halfHeight),
        };

        int[] triangles = new int[]{
            0, 2, 1,
            0, 3, 2
        };

        Mesh mesh = new()
        {
            name = "Quad"
        };
        mesh.SetVertices(points);
        mesh.triangles = triangles;

        meshFilter.sharedMesh = mesh;
        meshRenderer.sharedMaterial = _material;
    }
}