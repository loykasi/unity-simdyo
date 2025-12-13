using System.Collections.Generic;
using UnityEngine;

public class BoxBorder : MonoBehaviour
{
    // [SerializeField] private LineRenderer _line;
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _renderer;

    private List<Vector3> _vertices = new(8);
    private readonly float _width = 0.05f;
    private readonly int _sizeProperty = Shader.PropertyToID("_Size");

    private void Awake()
    {
        // _meshFilter.sharedMesh = ShapeGenerator.Instance.GenerateBoxBorder(_width);
        _meshFilter.sharedMesh = ShapeGenerator.Instance.GenerateBoxMesh("Border", Vector2.one, _vertices);
        _meshFilter.mesh.GetVertices(_vertices);
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

    public void SetBorder(float width, float height)
    {
        Vector2 size = new(width, height);
        _renderer.material.SetVector(_sizeProperty, size * 0.5f);
        // _line.positionCount = points.Length;
        // _line.SetPositions(points);

        // ShapeGenerator.Instance.GenerateBoxBorder(_vertices, width, height, _width);
        ShapeGenerator.Instance.GenerateBoxVertices(_vertices, size);
        _meshFilter.mesh.SetVertices(_vertices);
        _meshFilter.mesh.RecalculateBounds();
    }
}