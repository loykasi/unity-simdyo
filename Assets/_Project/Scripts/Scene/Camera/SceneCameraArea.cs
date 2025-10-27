using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneCameraArea : MonoBehaviour
{
    public Vector2 Size;
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _meshRenderer;

    private List<Vector3> _boxPoints = new List<Vector3>();
    private int[] _boxTriangles = new int[6];
    private Vector2[] _boxUV = new Vector2[4];

    private readonly int _sizeProperty = Shader.PropertyToID("_Size");

    private void OnValidate()
    {
        UpdateMesh();
    }

    [ContextMenu("Create Mesh")]
    private void CreateMesh()
    {
        Vector2 halfSize = Size / 2f;

        GenerateBoxVertices(halfSize);
        GenerateBoxTriangles();
        GenerateBoxUV();

        Mesh mesh = new()
        {
            name = "Quad"
        };
        mesh.SetVertices(_boxPoints);
        mesh.triangles = _boxTriangles;
        mesh.uv = _boxUV;

        _meshFilter.sharedMesh = mesh;
    }
    
    private void GenerateBoxVertices(Vector2 halfSize)
    {
        _boxPoints.Clear();
        _boxPoints.Add(new Vector3(halfSize.x, halfSize.y));
        _boxPoints.Add(new Vector3(- halfSize.x, halfSize.y));
        _boxPoints.Add(new Vector3(- halfSize.x, - halfSize.y));
        _boxPoints.Add(new Vector3(halfSize.x, - halfSize.y));
    }

    private void GenerateBoxTriangles()
    {
        System.Array.Clear(_boxTriangles, 0, _boxTriangles.Length);
        _boxTriangles[0] = 0;
        _boxTriangles[1] = 2;
        _boxTriangles[2] = 1;
        _boxTriangles[3] = 0;
        _boxTriangles[4] = 3;
        _boxTriangles[5] = 2;
    }

    private void GenerateBoxUV()
    {
        System.Array.Clear(_boxUV, 0, _boxUV.Length);
        _boxUV[0] = new Vector2(1f, 1f);
        _boxUV[1] = new Vector2(0f, 1f);
        _boxUV[2] = new Vector2(0f, 0f);
        _boxUV[3] = new Vector2(1f, 0f);
    }

    [ContextMenu("Update Mesh")]
    private void UpdateMesh()
    {
        if (_meshFilter == null || _meshFilter.sharedMesh == null)
        {
            return;
        }

        Vector2 halfSize = Size / 2f;

        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(halfSize.x, halfSize.y),
            new Vector3(- halfSize.x, halfSize.y),
            new Vector3(- halfSize.x, - halfSize.y),
            new Vector3(halfSize.x, - halfSize.y),
        };
        _meshFilter.sharedMesh.vertices = vertices;
        _meshFilter.sharedMesh.RecalculateBounds();


        _meshRenderer.sharedMaterial.SetVector(_sizeProperty, halfSize);
    }

    public void SetSize(Vector2 size)
    {
        Size = size;
        UpdateMesh();
    }
}