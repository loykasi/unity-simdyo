using System;
using UnityEngine;

public class GridOverlay : MonoBehaviour
{
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _renderer;

    private Camera _camera;
    private Vector3 _previous;
    private float _previousHeight;

    private readonly int _sizeProperty = Shader.PropertyToID("_Size");
    private readonly int _subSizeProperty = Shader.PropertyToID("_SubSize");
    private readonly int _colorProperty = Shader.PropertyToID("_Color");
    private readonly int _secondaryColorProperty = Shader.PropertyToID("_SecondaryColor");

    private void Start()
    {
        _camera = EngineManager.Instance.EditorCamera;
    }

    private void Update()
    {
        float halfHeight = _camera.orthographicSize;

        if (_previous == _camera.transform.position && _previousHeight == _camera.orthographicSize)
        {
            return;
        }

        float halfWidth = halfHeight * _camera.aspect;

        Vector3 position = new Vector3(_camera.transform.position.x, _camera.transform.position.y, 0f);
        transform.position = position;
        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(- halfWidth, halfHeight),
            new Vector3(halfWidth, halfHeight),
            new Vector3(- halfWidth, - halfHeight),
            new Vector3(halfWidth, - halfHeight),
        };
        _meshFilter.mesh.vertices = vertices;

        _previous = _camera.transform.position;
        _previousHeight = halfHeight;
    }

    public void Toggle(bool value)
    {
        gameObject.SetActive(value);
    }

    public void SetGridSize(float size, float subSize)
    {
        _renderer.material.SetFloat(_sizeProperty, size);
        _renderer.material.SetFloat(_subSizeProperty, subSize);
    }

    public void SetColor(Color color, Color secondaryColor)
    {
        _renderer.material.SetColor(_colorProperty, color);
        _renderer.material.SetColor(_secondaryColorProperty, secondaryColor);
    }
}