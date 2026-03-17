using System;
using UnityEngine;

public class MeshWrapper
{
    public readonly Mesh Mesh;
    
    public readonly Vector3[] Vertices;
    public readonly int[] Triangles;
    public readonly Vector2[] UV;

    public readonly int VertexCount;
    public readonly int TrisCount;
    public readonly int UVCount;

    public MeshWrapper(int vertexCount, int trisCount, int uvCount)
    {
        Mesh = new Mesh();

        VertexCount = vertexCount;
        TrisCount = trisCount;
        UVCount = uvCount;

        Vertices = new Vector3[VertexCount];
        Triangles = new int[TrisCount];
        UV = new Vector2[UVCount];
    }

    public MeshWrapper(MeshWrapper meshWrapper)
    {
        Mesh = new Mesh();

        VertexCount = meshWrapper.VertexCount;
        TrisCount = meshWrapper.TrisCount;
        UVCount = meshWrapper.UVCount;

        Vertices = new Vector3[VertexCount];
        Triangles = new int[TrisCount];
        UV = new Vector2[UVCount];
        Array.Copy(meshWrapper.Vertices, Vertices, VertexCount);
        Array.Copy(meshWrapper.Triangles, Triangles, TrisCount);
        Array.Copy(meshWrapper.UV, UV, UVCount);
        Update();
    }

    public void AssignTo(MeshFilter meshFilter)
    {
        meshFilter.sharedMesh = Mesh;
    }

    public void Update()
    {
        Mesh.vertices = Vertices;
        Mesh.triangles = Triangles;
        Mesh.uv = UV;

        Mesh.RecalculateBounds();
    }
}