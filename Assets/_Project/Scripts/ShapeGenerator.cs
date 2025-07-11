using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator : MonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField] private Material _circleMaterial;
    [SerializeField] private float _radius;
    [SerializeField] private float _totalVert;

    private readonly int _radiusProperty = Shader.PropertyToID("_Radius");

    public void AddBox()
    {
        GameObject shape = new();
        shape.name = "Box";
        MeshFilter meshFilter = shape.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = shape.AddComponent<MeshRenderer>();

        List<Vector3> points = new List<Vector3>()
        {
            new Vector3(-1, 1),
            new Vector3(1, 1),
            new Vector3(-1, -1),
            new Vector3(1, -1),
        };

        int[] triangles = new int[]{
            2, 0, 1,
            2, 1, 3
        };

        Mesh mesh = new()
        {
            name = "Quad"
        };
        mesh.SetVertices(points);
        mesh.triangles = triangles;

        meshFilter.sharedMesh = mesh;
        meshRenderer.material = _material;
        meshRenderer.material.color = Random.ColorHSV();
    }

    public void AddCircle()
    {
        GameObject shape = new();
        shape.name = "Circle";
        MeshFilter meshFilter = shape.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = shape.AddComponent<MeshRenderer>();

        float vertRadius = _radius / Mathf.Cos(Mathf.PI / _totalVert);
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
        meshRenderer.material = _circleMaterial;
        meshRenderer.material.color = Random.ColorHSV();
        meshRenderer.material.SetFloat(_radiusProperty, _radius);
    }
}