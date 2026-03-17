using UnityEngine;

public static class BoxMeshGenerator
{
    public static MeshWrapper Generate(Vector2 size)
    {
        MeshWrapper mesh = new(4, 6, 4);

        GenerateVertices(mesh.Vertices, size);
        GenerateTriangles(mesh.Triangles);
        GenerateUV(mesh.UV);

        mesh.Update();

        return mesh;
    }

    public static void GenerateVertices(Vector3[] vertices, Vector2 size)
    {
        Vector2 halfSize = size / 2f;
        vertices[0] = new Vector3(halfSize.x, halfSize.y);
        vertices[1] = new Vector3(- halfSize.x, halfSize.y);
        vertices[2] = new Vector3(- halfSize.x, - halfSize.y);
        vertices[3] = new Vector3(halfSize.x, - halfSize.y);
    }

    private static void GenerateTriangles(int[] triangles)
    {
        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 1;
        triangles[3] = 0;
        triangles[4] = 3;
        triangles[5] = 2;
    }

    private static void GenerateUV(Vector2[] uv)
    {
        uv[0] = new Vector2(1f, 1f);
        uv[1] = new Vector2(0f, 1f);
        uv[2] = new Vector2(0f, 0f);
        uv[3] = new Vector2(1f, 0f);
    }
}