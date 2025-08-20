using UnityEngine;

public class CircleEntity : SceneEntity
{
    public override EntityType EntityType => EntityType.Circle;
    public override Bounds Bounds => _bounds;
    private Bounds _bounds = new();

    public CircleBorder Border;

    public int TotalVert;
    public float Radius;

    private readonly int _radiusProperty = Shader.PropertyToID("_Radius");

    public void SetRadius(float radius, int totalVert)
    {
        Radius = radius;
        TotalVert = totalVert;
        ((CircleCollider2D)Collider).radius = radius;

        float vertRadius = radius / Mathf.Cos(Mathf.PI / TotalVert);
        Vector3[] vertices = new Vector3[TotalVert];
        for (int i = 0; i < TotalVert; i++)
        {
            float x = vertRadius * Mathf.Sin(i * 2 * Mathf.PI / TotalVert);
            float y = vertRadius * Mathf.Cos(i * 2 * Mathf.PI / TotalVert);
            vertices[i] = new Vector3(x, y, 0f);
        }
        MeshFilter.mesh.vertices = vertices;
        Renderer.material.SetFloat(_radiusProperty, radius);

        _bounds.center = Position;
        _bounds.size = new Vector3(Radius * 2f, Radius * 2f);
    }

    public void UpdateCircle(Vector3 from, Vector3 to)
    {
        Vector3 center = from;
        Radius = Vector3.Distance(from, to);

        transform.position = center;

        float vertRadius = Radius / Mathf.Cos(Mathf.PI / TotalVert);
        Vector3[] vertices = new Vector3[TotalVert];
        for (int i = 0; i < TotalVert; i++)
        {
            float x = vertRadius * Mathf.Sin(i * 2 * Mathf.PI / TotalVert);
            float y = vertRadius * Mathf.Cos(i * 2 * Mathf.PI / TotalVert);
            vertices[i] = new Vector3(x, y, 0f);
        }
        MeshFilter.mesh.vertices = vertices;
        MeshFilter.mesh.RecalculateBounds();
        Renderer.material.SetFloat(_radiusProperty, Radius);

        ((CircleCollider2D)Collider).radius = Radius;

        _bounds.center = Position;
        _bounds.size = new Vector3(Radius * 2f, Radius * 2f);

        Border.SetRadius(Radius);
    }

    public override void Select()
    {
        Border.Enable();
        Border.SetRadius(Radius);
    }

    public override void Deselect()
    {
        Border.Disable();
    }
}