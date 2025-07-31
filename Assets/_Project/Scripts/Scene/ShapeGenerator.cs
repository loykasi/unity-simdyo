using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator : Singleton<ShapeGenerator>
{
    [SerializeField] private Material _material;
    [SerializeField] private Material _circleMaterial;
    [SerializeField] private float _totalVert;

    private readonly int _radiusProperty = Shader.PropertyToID("_Radius");

    public void AddBox(Vector3 from, Vector3 to)
    {
        if (from == to)
        {
            return;
        }

        GameObject shape = new()
        {
            name = "Box"
        };
        MeshFilter meshFilter = shape.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = shape.AddComponent<MeshRenderer>();
        BoxCollider2D collider = shape.AddComponent<BoxCollider2D>();

        Vector3 center = (from + to) / 2f;
        float halfWidth = Mathf.Abs(from.x - to.x) / 2f;
        float halfHeight = Mathf.Abs(from.y - to.y) / 2f;

        shape.transform.position = center;

        List<Vector3> points = new List<Vector3>()
        {
            new Vector3(- halfWidth, halfHeight),
            new Vector3(halfWidth, halfHeight),
            new Vector3(- halfWidth, - halfHeight),
            new Vector3(halfWidth, - halfHeight),
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

        collider.size = new Vector2(halfWidth * 2, halfHeight * 2);

        VisualScripting vs = shape.AddComponent<VisualScripting>();
        SceneEntity sceneEntity = shape.AddComponent<SceneEntity>();
        sceneEntity.VisualScripting = vs;
    }

    [ContextMenu("AddCircle")]
    public void AddCircle(Vector3 from, Vector3 to)
    {
        GameObject shape = new()
        {
            name = "Circle"
        };

        MeshFilter meshFilter = shape.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = shape.AddComponent<MeshRenderer>();
        CircleCollider2D collider = shape.AddComponent<CircleCollider2D>();

        Vector3 center = (from + to) / 2f;
        float radius = Vector3.Distance(from, to);

        shape.transform.position = center;

        float vertRadius = radius / Mathf.Cos(Mathf.PI / _totalVert);
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
        meshRenderer.material.SetFloat(_radiusProperty, radius);

        collider.radius = radius;

        VisualScripting vs = shape.AddComponent<VisualScripting>();
        SceneEntity sceneEntity = shape.AddComponent<SceneEntity>();
        sceneEntity.VisualScripting = vs;
    }
}