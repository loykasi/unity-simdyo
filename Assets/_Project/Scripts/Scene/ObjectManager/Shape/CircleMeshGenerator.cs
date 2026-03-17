using GameCore.Extensions;
using UnityEngine;

public static class CircleMeshGenerator
{
    public static MeshWrapper Generate(float radius)
    {
        int totalVert = ShapeGenerator.TotalVert;
        MeshWrapper mesh = new(totalVert, (totalVert - 2) * 3, totalVert);
        
        GenerateVertices(mesh.Vertices, radius);
        GenerateTriangles(mesh.Triangles);
        GenerateUV(mesh.UV);

        mesh.Update();

        return mesh;
    }

    public static void GenerateVertices(Vector3[] vertices, float radius)
    {
        int length = ShapeGenerator.TotalVert;
        float angleStep = 2 * Mathf.PI / length;
        float vertRadius = radius / Mathf.Cos(Mathf.PI / length);
        for (int i = 0; i < length; i++)
        {
            float angle = i * angleStep;
            vertices[i] = new Vector3
            (
                vertRadius * Mathf.Sin(angle),
                vertRadius * Mathf.Cos(angle),
                0f
            );
        }
    }

    private static void GenerateTriangles(int[] triangles)
    {
        int length = ShapeGenerator.TotalVert - 2;
        for (int i = 0; i < length; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }
    }

    private static void GenerateUV(Vector2[] uv)
    {
        int totalVert = ShapeGenerator.TotalVert;
        for (int i = 0; i < totalVert; i++)
        {
            float angle = i * 2 * Mathf.PI / totalVert;
            float x = 1f * Mathf.Sin(angle);
            float y = 1f * Mathf.Cos(angle);
            x = x.MapRange(-1f, 1, 0f, 1f);
            y = y.MapRange(-1f, 1, 0f, 1f);
            uv[i] = new Vector2(x, y);
        }
    }
}
