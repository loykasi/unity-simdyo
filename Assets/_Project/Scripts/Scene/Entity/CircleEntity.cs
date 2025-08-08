using UnityEngine;

public class CircleEntity : SceneEntity
{
    private readonly int _radiusProperty = Shader.PropertyToID("_Radius");

    public int TotalVert;
    public float Radius;

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
        Renderer.material.SetFloat(_radiusProperty, Radius);

        ((CircleCollider2D)Collider).radius = Radius;
    }
}