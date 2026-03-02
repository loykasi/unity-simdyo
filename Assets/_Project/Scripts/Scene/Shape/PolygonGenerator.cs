using GameCore.Extensions;
using LibTessDotNet;
using UnityEngine;

class PolygonGenerator
{
    private Tess _tess = new();

    public UnityEngine.Mesh AddPolygon(Vector2[] points)
    {
        // triangulate and create mesh with libtessdotnet
        var contour = new ContourVertex[points.Length];

        float maxX = Mathf.NegativeInfinity;
        float maxY = Mathf.NegativeInfinity;
        float minX = Mathf.Infinity;
        float minY = Mathf.Infinity;
        for (int i = 0; i < points.Length; i++)
        {
            contour[i].Position = new Vec3(points[i].x, points[i].y, 0f);
            contour[i].Data = i;

            maxX = Mathf.Max(maxX, points[i].x);
            maxY = Mathf.Max(maxY, points[i].y);
            minX = Mathf.Min(minX, points[i].x);
            minY = Mathf.Min(minY, points[i].y);
        }
        _tess.AddContour(contour, ContourOrientation.CounterClockwise);
        _tess.Tessellate(WindingRule.EvenOdd, ElementType.Polygons, 3);

        Vector3[] verts = new Vector3[_tess.VertexCount];
        for (int i = 0; i < _tess.VertexCount; i++)
        {
            Vec3 v = _tess.Vertices[i].Position;
            verts[i] = new Vector3(v.X, v.Y, v.Z);
        }

        // uv
        // Debug.Log($"UV: {minX} - {maxX} | {minY} - {maxY}");
        Vector2[] uv = new Vector2[verts.Length];
        for (int i = 0; i < verts.Length; i++)
        {
            float x = verts[i].x.MapRange(minX, maxX, 0f, 1f);
            float y = verts[i].y.MapRange(minY, maxY, 0f, 1f);
            uv[i] = new Vector2(x, y);
        }

        UnityEngine.Mesh mesh = new UnityEngine.Mesh();
        mesh.vertices = verts;
        mesh.triangles = _tess.Elements;
        mesh.uv = uv;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        // // union
        // int count = points.Count;
        // double[] dpoints = new double[count * 2];
        // for (int i = 0; i < count; i++)
        // {
        //     dpoints[i * 2] = points[i].x;
        //     dpoints[i * 2 + 1] = points[i].y;
        // }

        // Debug.Log($"path: {string.Join("|", dpoints)}");

        // PathsD paths = new()
        // {
        //     Clipper.MakePath(dpoints)
        // };
        // PathsD solution = Clipper.Union(paths, FillRule.EvenOdd);

        // foreach (var path in solution)
        // {
        //     foreach (var point in path)
        //     {
        //         Debug.Log($"solution: {point.x}, {point.y}");
        //     }
        // }

        return mesh;
    }
}