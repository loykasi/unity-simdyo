using System;
using System.Collections.Generic;
using UnityEngine;

public class PolygonBorder : MonoBehaviour
{
    public struct Edge {
        public int A, B;
        public Edge(int a, int b) {
            A = Mathf.Min(a, b);
            B = Mathf.Max(a, b);
        }
    }

    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _renderer;

    private List<Vector3> _vertices = new();
    private List<Edge> _edges = new();

    private readonly int _colorProperty = Shader.PropertyToID("_Color");

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Enable(ColorHSV color)
    {
        float x = Mathf.Max(color.V - 0.75f, 0) / 0.25f;
        float threshold = Mathf.Lerp(0f, 0.35f, x);

        if (color.S < threshold)
        {
            _renderer.material.SetColor(_colorProperty, Color.black);
        }
        else
        {
            _renderer.material.SetColor(_colorProperty, Color.white);
        }

        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void SetMesh(Mesh mesh, List<Vector3> points)
    {
        mesh.GetVertices(_vertices);
        // _vertices.AddRange(points);
        // CreateOutlineVector(mesh);
        _meshFilter.sharedMesh = mesh;
    }

    private void CreateOutlineVector(Mesh mesh)
    {
        GenerateConnectedEdges(mesh);

        int count = _vertices.Count;
        Vector4[] tangents = new Vector4[count];
        for (int i = 0; i < count; i++)
        {
            Vector3 previous = _vertices[(i - 1 + count) % count];
            Vector3 current = _vertices[i];
            Vector3 next = _vertices[(i + 1) % count];

            Debug.Log($"{current + transform.position}");

            Vector3 vec1 = (current - previous).normalized;
            Vector3 vec2 = (next - current).normalized;
            
            Vector3 normal1 = new(- vec1.y, vec1.x);
            Vector3 normal2 = new(- vec2.y, vec2.x);

            if (IsLineIntersect(previous + normal1, current + normal1, current + normal2, next + normal2, out Vector3 intersection))
            {
                Vector3 tangent = (intersection - current).normalized;
                tangents[i] = new Vector4
                (
                    tangent.x,
                    tangent.y,
                    0,
                    1
                );
            }
            else
            {
                // fallback
                tangents[i] = normal1;
            }
        }

        mesh.SetTangents(tangents);

        foreach (var tangent in mesh.tangents)
        {
            Debug.DrawRay(transform.position, tangent, Color.yellow, 10f);            
        }
    }

    private void GenerateConnectedEdges(Mesh mesh)
    {
        int[] triangles = mesh.triangles;
        for (int i = 0; i < triangles.Length; i += 3) {
            _edges.Add(new Edge(triangles[i], triangles[i + 1]));
            _edges.Add(new Edge(triangles[i + 1], triangles[i + 2]));
            _edges.Add(new Edge(triangles[i + 2], triangles[i]));
        }
    }

    // https://en.wikipedia.org/wiki/Line%E2%80%93line_intersection#Given_two_points_on_each_line
    private bool IsLineIntersect(
        Vector3 a, Vector3 b,
        Vector3 c, Vector3 d,
        out Vector3 intersection)
    {
        Vector3 v1 = a - b;
        Vector3 v2 = c - d;
        float den = v1.x * v2.y - v1.y * v2.x;

        if (Mathf.Abs(den) < float.Epsilon)
        {
            intersection = default;
            return false;
        }

        float c1 = a.x * b.y - a.y * b.x;
        float c2 = c.x * d.y - c.y * d.x;
        intersection = new Vector3
        (
            (c1 * v2.x - v1.x * c2) / den,
            (c1 * v2.y - v1.y * c2) / den,
            0f
        );
        return true;
    }
}