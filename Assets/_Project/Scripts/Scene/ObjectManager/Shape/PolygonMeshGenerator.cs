using GameCore.Extensions;
using LibTessDotNet;
using UnityEngine;

class PolygonMeshGenerator
{
    public static MeshWrapper Generate(Vector2[] points)
    {
        // triangulate and create mesh with libtessdotnet
        Tess tess = new();
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
        tess.AddContour(contour, ContourOrientation.CounterClockwise);
        tess.Tessellate(WindingRule.EvenOdd, ElementType.Polygons, 3);

        MeshWrapper meshWrapper = new(tess.VertexCount, tess.Elements.Length, tess.VertexCount);

        for (int i = 0; i < tess.VertexCount; i++)
        {
            Vec3 v = tess.Vertices[i].Position;
            meshWrapper.Vertices[i] = new Vector3(v.X, v.Y, v.Z);
        }

        for (int i = 0; i < tess.Elements.Length; i++)
        {
            meshWrapper.Triangles[i] = tess.Elements[i];
        }

        for (int i = 0; i < tess.VertexCount; i++)
        {
            float x = tess.Vertices[i].Position.X.MapRange(minX, maxX, 0f, 1f);
            float y = tess.Vertices[i].Position.Y.MapRange(minY, maxY, 0f, 1f);
            meshWrapper.UV[i] = new Vector2(x, y);
        }

        meshWrapper.Update();
        return meshWrapper;
    }
}