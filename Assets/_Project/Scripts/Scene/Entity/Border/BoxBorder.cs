using System.Collections.Generic;
using UnityEngine;

public class BoxBorder : MonoBehaviour
{
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _renderer;

    private readonly int _sizeProperty = Shader.PropertyToID("_Size");

    private void Awake()
    {
        Disable();
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void SetMesh(MeshWrapper mesh)
    {
        mesh.AssignTo(_meshFilter);
    }

    public void SetSize(Vector2 size)
    {
        _renderer.material.SetVector(_sizeProperty, size * 0.5f);

        
        // _renderer.material.SetVector(_sizeProperty, size * 0.5f);
        // // _line.positionCount = points.Length;
        // // _line.SetPositions(points);

        // // ShapeGenerator.Instance.GenerateBoxBorder(_vertices, width, height, _width);
        // ShapeGenerator.Instance.GenerateBoxVertices(_vertices, size);
        // _meshFilter.mesh.SetVertices(_vertices);
        // _meshFilter.mesh.RecalculateBounds();
    }
}