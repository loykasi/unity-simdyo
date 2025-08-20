using System.Collections.Generic;
using UnityEngine;

public class CircleBorder : MonoBehaviour
{
    public MeshFilter MeshFilter;
    public MeshRenderer Renderer;

    private readonly int _radiusProperty = Shader.PropertyToID("_Radius");
    private readonly float _width = 0.05f;

    private List<Vector3> _vertices = new(20);

    private void Awake()
    {
        MeshFilter.sharedMesh = ShapeGenerator.Instance.GenerateRing(1f, _width);
        MeshFilter.mesh.GetVertices(_vertices);
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

    public void SetRadius(float radius)
    {
        Renderer.material.SetFloat(_radiusProperty, radius);

        ShapeGenerator.Instance.GenerateRingVertices(_vertices, radius, _width);
        MeshFilter.mesh.SetVertices(_vertices);
        MeshFilter.mesh.RecalculateBounds();
    }
}