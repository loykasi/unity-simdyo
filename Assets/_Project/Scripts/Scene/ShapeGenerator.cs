using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator : Singleton<ShapeGenerator>
{
    [SerializeField] private BoxEntity _boxEntityPrefab;
    [SerializeField] private CircleEntity _circleEntityPrefab;
    [SerializeField] private SceneEntity _sceneEntityPrefab;
    [SerializeField] private Material _material;
    [SerializeField] private Material _circleMaterial;
    [SerializeField] private int _totalVert;

    private readonly int _radiusProperty = Shader.PropertyToID("_Radius");

    public BoxEntity AddBox(Vector3 from, Vector3 to)
    {
        if (from == to)
        {
            return null;
        }

        Vector3 position = (from + to) / 2f;
        float width = Mathf.Abs(from.x - to.x);
        float height = Mathf.Abs(from.y - to.y);

        if (width == 0 || height == 0)
        {
            return null;
        }

        return AddBox(position, width, height);
    }

    public BoxEntity AddBox(Vector3 position, float width, float height)
    {

        BoxEntity sceneEntity = Instantiate(_boxEntityPrefab);
        sceneEntity.name = "Box";

        float halfWidth = width / 2f;
        float halfHeight = height / 2f;

        sceneEntity.transform.position = position;

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

        sceneEntity.MeshFilter.sharedMesh = mesh;
        sceneEntity.Renderer.material = _material;
        sceneEntity.SetSize(width, height);
        sceneEntity.CurrentColor = GetRandomColor();

        ObjectManager.Instance.AddEntity(sceneEntity);
        Physics2D.SyncTransforms();

        return sceneEntity;
    }

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
        CircleEntity sceneEntity = Instantiate(_circleEntityPrefab);
        sceneEntity.name = "Circle";

        sceneEntity.transform.position = position;

        float vertRadius = radius / Mathf.Cos(Mathf.PI / _totalVert);
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < _totalVert; i++)
        {
            float x = vertRadius * Mathf.Sin(i * 2 * Mathf.PI / _totalVert);
            float y = vertRadius * Mathf.Cos(i * 2 * Mathf.PI / _totalVert);
            points.Add(new Vector3(x, y, 0f));
        }

        List<int> trianglesList = new List<int>();
        for (int i = 0; i < _totalVert - 2; i++)
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

        sceneEntity.MeshFilter.sharedMesh = mesh;
        sceneEntity.Renderer.material = _circleMaterial;
        sceneEntity.Renderer.material.SetFloat(_radiusProperty, radius);
        sceneEntity.SetRadius(radius, _totalVert);
        sceneEntity.CurrentColor = GetRandomColor();

        ObjectManager.Instance.AddEntity(sceneEntity);
        Physics2D.SyncTransforms();

        return sceneEntity;
    }

    private ColorHSV GetRandomColor()
    {
        return new ColorHSV
        (
            (float)Random.Range(0, 361) / 360,
            (float)Random.Range(0, 100) / 100,
            (float)Random.Range(0, 100) / 100,
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

        float vertRadius = radius / Mathf.Cos(Mathf.PI / _totalVert);
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < _totalVert; i++)
        {
            float x = vertRadius * Mathf.Sin(i * 2 * Mathf.PI / _totalVert);
            float y = vertRadius * Mathf.Cos(i * 2 * Mathf.PI / _totalVert);
            points.Add(new Vector3(x, y, 0f));
        }

        List<int> trianglesList = new List<int>();
        for (int i = 0; i < _totalVert - 2; i++)
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

    public Mesh GenerateRing(float radius, float width)
    {
        float vertRadius = radius / Mathf.Cos(Mathf.PI / _totalVert);
        float innerRadius = radius - width;

        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < _totalVert; i++)
        {
            float sin = Mathf.Sin(i * 2 * Mathf.PI / _totalVert);
            float cos = Mathf.Cos(i * 2 * Mathf.PI / _totalVert);

            points.Add(new Vector3(vertRadius * sin, vertRadius * cos, 0f));
            points.Add(new Vector3(innerRadius * sin, innerRadius * cos, 0f));
        }

        List<int> trianglesList = new List<int>();
        int pointCount = _totalVert * 2;
        for (int i = 0; i < _totalVert; i++)
        {
            int startIndex = i * 2;
            trianglesList.Add(startIndex + 0);
            trianglesList.Add((startIndex + 2) % pointCount);
            trianglesList.Add(startIndex + 1);

            trianglesList.Add(startIndex + 1);
            trianglesList.Add((startIndex + 2) % pointCount);
            trianglesList.Add((startIndex + 3) % pointCount);
        }
        int[] triangles = trianglesList.ToArray();

        Mesh mesh = new()
        {
            name = "Ring"
        };
        mesh.SetVertices(points);
        mesh.triangles = triangles;

        return mesh;
    }

    public void GenerateRingVertices(List<Vector3> vertices, float radius, float width)
    {
        vertices.Clear();
        float vertRadius = radius / Mathf.Cos(Mathf.PI / _totalVert);
        float innerRadius = radius - width;

        for (int i = 0; i < _totalVert; i++)
        {
            float sin = Mathf.Sin(i * 2 * Mathf.PI / _totalVert);
            float cos = Mathf.Cos(i * 2 * Mathf.PI / _totalVert);

            vertices.Add(new Vector3(vertRadius * sin, vertRadius * cos, 0f));
            vertices.Add(new Vector3(innerRadius * sin, innerRadius * cos, 0f));
        }
    }

    public Mesh GenerateBoxBorder(float borderWidth)
    {
        Vector2 halfSize = new(0.5f, 0.5f);
        Vector2 innerHalfSize = halfSize - borderWidth * Vector2.one;

        List<Vector3> points = new()
        {
            new Vector3(halfSize.x, halfSize.y),
            new Vector3(innerHalfSize.x, innerHalfSize.y),

            new Vector3(- halfSize.x, halfSize.y),
            new Vector3(- innerHalfSize.x, innerHalfSize.y),

            new Vector3(- halfSize.x, - halfSize.y),
            new Vector3(- innerHalfSize.x, - innerHalfSize.y),

            new Vector3(halfSize.x, - halfSize.y),
            new Vector3(innerHalfSize.x, - innerHalfSize.y),
        };

        List<int> triangles = new();
        int totalPoint = 8;
        for (int i = 0; i < 4; i++)
        {
            int startIndex = i * 2;
            triangles.Add(startIndex);
            triangles.Add(startIndex + 1);
            triangles.Add((startIndex + 3) % totalPoint);

            triangles.Add(startIndex);
            triangles.Add((startIndex + 3) % totalPoint);
            triangles.Add((startIndex + 2) % totalPoint);
        }

        Mesh mesh = new()
        {
            name = "Box"
        };
        mesh.SetVertices(points);
        mesh.triangles = triangles.ToArray();

        return mesh;
    }

    public void GenerateBoxBorder(List<Vector3> vertices, float width, float height, float borderWidth)
    {
        vertices.Clear();

        Vector2 halfSize = new(width / 2f, height / 2f);
        Vector2 innerHalfSize = halfSize - borderWidth * Vector2.one;

        vertices.Add(new Vector3(halfSize.x, halfSize.y));
        vertices.Add(new Vector3(innerHalfSize.x, innerHalfSize.y));

        vertices.Add(new Vector3(- halfSize.x, halfSize.y));
        vertices.Add(new Vector3(- innerHalfSize.x, innerHalfSize.y));

        vertices.Add(new Vector3(- halfSize.x, - halfSize.y));
        vertices.Add(new Vector3(- innerHalfSize.x, - innerHalfSize.y));

        vertices.Add(new Vector3(halfSize.x, - halfSize.y));
        vertices.Add(new Vector3(innerHalfSize.x, - innerHalfSize.y));
    }
}