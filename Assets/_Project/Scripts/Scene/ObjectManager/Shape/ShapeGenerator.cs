using System.Collections.Generic;
using UnityEngine;
using GameCore.Extensions;
using LibTessDotNet;

public static class ShapeGenerator
{
    // ==== ADD BOX ====

    public static BoxEntity AddBox(Vector3 from, Vector3 to)
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

    public static BoxEntity AddBox(Vector3 position, float width, float height)
    {
        if (width == 0 || height == 0)
        {
            return null;
        }

        BoxEntity box = Object.Instantiate(ObjectManager.Instance.BoxPrefab);
        MeshWrapper mesh = BoxMeshGenerator.Generate(new Vector2(width, height));
        
        box.name = "Box";
        box.transform.position = position;
        box.SetMesh(mesh, ObjectManager.Instance.BoxMaterial, width, height);
        box.CurrentColor = GetRandomColor();

        Physics2D.SyncTransforms();
        return box;
    }

    public static BoxEntity Clone(BoxEntity entity)
    {
        BoxEntity box = Object.Instantiate(ObjectManager.Instance.BoxPrefab);
        MeshWrapper mesh = new(entity.Mesh);

        box.name = "Box";
        box.transform.position = entity.Position;
        box.SetMesh(mesh, ObjectManager.Instance.BoxMaterial, entity.Width, entity.Height);

        return box;
    }

    // ==== CIRCLE ====

    public static CircleEntity AddCircle(Vector3 from, Vector3 to)
    {
        if (from == to)
        {
            return null;
        }

        float radius = Vector3.Distance(from, to);

        return AddCircle(from, radius);
    }

    public static CircleEntity AddCircle(Vector3 position, float radius)
    {
        if (radius == 0)
        {
            return null;
        }

        CircleEntity sceneEntity = Object.Instantiate(ObjectManager.Instance.CirclePrefab);
        MeshWrapper mesh = CircleMeshGenerator.Generate(radius);

        sceneEntity.name = "Circle";
        sceneEntity.transform.position = position;
        sceneEntity.SetMesh(mesh, ObjectManager.Instance.CircleMaterial, radius);
        sceneEntity.CurrentColor = GetRandomColor();

        Physics2D.SyncTransforms();

        return sceneEntity;
    }

    public static CircleEntity Clone(CircleEntity entity)
    {
        CircleEntity circle = Object.Instantiate(ObjectManager.Instance.CirclePrefab);
        MeshWrapper mesh = new(entity.Mesh);

        circle.name = "Circle";
        circle.transform.position = entity.Position;
        circle.SetMesh(mesh, ObjectManager.Instance.CircleMaterial, entity.Radius);

        return circle;
    }

    // ==== ADD POLYGON ====

    public static PolygonEntity AddPolygon(List<Vector3> points)
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

    public static PolygonEntity AddPolygon(Vector3 position, Vector2[] points)
    {
        if (points.Length <= 2) return null;

        MeshWrapper meshWrapper = GeneratePolygonMesh(points);

        PolygonEntity sceneEntity = Object.Instantiate(ObjectManager.Instance.PolygonPrefab);
        sceneEntity.name = "Polygon";
        sceneEntity.transform.position = position;
        sceneEntity.SetMesh(meshWrapper, ObjectManager.Instance.PolygonMaterial, points);
        sceneEntity.CurrentColor = GetRandomColor();

        Physics2D.SyncTransforms();
        
        return sceneEntity;
    }

    public static MeshWrapper GeneratePolygonMesh(Vector2[] points)
    {
        // triangulate and create mesh with libtessdotnet
        Tess tess = new();
        var contour = new ContourVertex[points.Length];

        float maxX = Mathf.NegativeInfinity;
        float maxY = Mathf.NegativeInfinity;
        float minX = Mathf.Infinity;
        float minY = Mathf.Infinity;
        for (int i = 0; i < points.Length; i++)
        {
            contour[i].Position = new Vec3(points[i].x, points[i].y, 0f);
            contour[i].Data = i;

            maxX = Mathf.Max(maxX, points[i].x);
            maxY = Mathf.Max(maxY, points[i].y);
            minX = Mathf.Min(minX, points[i].x);
            minY = Mathf.Min(minY, points[i].y);
        }
        tess.AddContour(contour, ContourOrientation.CounterClockwise);
        tess.Tessellate(WindingRule.EvenOdd, ElementType.Polygons, 3);

        MeshWrapper meshWrapper = new(tess.VertexCount, tess.Elements.Length, tess.VertexCount);

        for (int i = 0; i < tess.VertexCount; i++)
        {
            Vec3 v = tess.Vertices[i].Position;
            meshWrapper.Vertices[i] = new Vector3(v.X, v.Y, v.Z);
        }

        for (int i = 0; i < tess.Elements.Length; i++)
        {
            meshWrapper.Triangles[i] = tess.Elements[i];
        }

        for (int i = 0; i < tess.VertexCount; i++)
        {
            float x = tess.Vertices[i].Position.X.MapRange(minX, maxX, 0f, 1f);
            float y = tess.Vertices[i].Position.Y.MapRange(minY, maxY, 0f, 1f);
            meshWrapper.UV[i] = new Vector2(x, y);
        }

        meshWrapper.Update();
        return meshWrapper;
    }

    public static PolygonEntity Clone(PolygonEntity entity)
    {
        PolygonEntity sceneEntity = Object.Instantiate(ObjectManager.Instance.PolygonPrefab);
        MeshWrapper mesh = new(entity.Mesh);
        
        int length = entity.PolygonPoints.Length;
        Vector2[] points = new Vector2[length];
        System.Array.Copy(entity.PolygonPoints, points, length);

        sceneEntity.name = "Polygon";
        sceneEntity.transform.position = entity.Position;
        sceneEntity.SetMesh(mesh, ObjectManager.Instance.PolygonMaterial, points);

        Physics2D.SyncTransforms();
        
        return sceneEntity;
    }

    private static ColorHSV GetRandomColor()
    {
        return new ColorHSV
        (
            (float)Random.Range(0, 361) / 360,
            (float)Random.Range(0, 101) / 100,
            (float)Random.Range(0, 101) / 100,
            1f
        );
    }

    // private static void AddCircle()
    // {
    //     GameObject shape = new()
    //     {
    //         name = "Circle"
    //     };

    //     MeshFilter meshFilter = shape.AddComponent<MeshFilter>();
    //     MeshRenderer meshRenderer = shape.AddComponent<MeshRenderer>();

    //     Vector3 center = Vector3.zero;
    //     float radius = 1f;

    //     shape.transform.position = center;

    //     int totalVert = GameConstants.CircleVertexCount;
    //     float vertRadius = radius / Mathf.Cos(Mathf.PI / totalVert);
    //     List<Vector3> points = new List<Vector3>();
    //     for (int i = 0; i < totalVert; i++)
    //     {
    //         float x = vertRadius * Mathf.Sin(i * 2 * Mathf.PI / totalVert);
    //         float y = vertRadius * Mathf.Cos(i * 2 * Mathf.PI / totalVert);
    //         points.Add(new Vector3(x, y, 0f));
    //     }

    //     List<int> trianglesList = new List<int>();
    //     for (int i = 0; i < totalVert - 2; i++)
    //     {
    //         trianglesList.Add(0);
    //         trianglesList.Add(i + 1);
    //         trianglesList.Add(i + 2);
    //     }
    //     int[] triangles = trianglesList.ToArray();

    //     Mesh mesh = new()
    //     {
    //         name = "Circle"
    //     };
    //     mesh.SetVertices(points);
    //     mesh.triangles = triangles;

    //     meshFilter.sharedMesh = mesh;
    //     meshRenderer.sharedMaterial = _circleMaterial;
    // }

    // private static void AddBox()
    // {
    //     Vector3 from = Vector3.zero;
    //     Vector3 to = new(1f, 1f);

    //     GameObject shape = new()
    //     {
    //         name = "Box"
    //     };
    //     MeshFilter meshFilter = shape.AddComponent<MeshFilter>();
    //     MeshRenderer meshRenderer = shape.AddComponent<MeshRenderer>();

    //     Vector3 center = (from + to) / 2f;
    //     float halfWidth = Mathf.Abs(from.x - to.x) / 2f;
    //     float halfHeight = Mathf.Abs(from.y - to.y) / 2f;

    //     shape.transform.position = center;

    //     List<Vector3> points = new List<Vector3>()
    //     {
    //         new Vector3(halfWidth, halfHeight),
    //         new Vector3(- halfWidth, halfHeight),
    //         new Vector3(- halfWidth, - halfHeight),
    //         new Vector3(halfWidth, - halfHeight),
    //     };

    //     int[] triangles = new int[]{
    //         0, 2, 1,
    //         0, 3, 2
    //     };

    //     Mesh mesh = new()
    //     {
    //         name = "Quad"
    //     };
    //     mesh.SetVertices(points);
    //     mesh.triangles = triangles;

    //     meshFilter.sharedMesh = mesh;
    //     meshRenderer.sharedMaterial = _material;
    // }
}