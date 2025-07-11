using UnityEngine;
using UnityEngine.UI;

public class UILineRenderer : MaskableGraphic
{
    public Vector3[] Points;

    public float Thickness;

    public void Init(int pointCount)
    {
        Points = new Vector3[pointCount];
    }

    public void UpdateVertex()
    {
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (Points == null || Points.Length < 2)
            return;

        for (int i = 0; i < Points.Length - 1; i++)
        {
            DrawLine(Points[i], Points[i + 1], vh);

            int index = i * 5;
            vh.AddTriangle(index + 0, index + 3, index + 1);
            vh.AddTriangle(index + 3, index + 0, index + 2);

            if (i != 0)
            {
                vh.AddTriangle(index - 0, index - 1, index - 3);
                vh.AddTriangle(index + 1, index - 1, index - 2);
            }
        }
    }

    private void DrawLine(Vector3 point1, Vector3 point2, VertexHelper vh)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        Quaternion rotation = Quaternion.Euler(0, 0, GetAngleTowardTarget(point1, point2) - 90f);
        
        vertex.position = point1 + rotation * new Vector3(-Thickness / 2.0f, 0f);
        vh.AddVert(vertex);
        vertex.position = point1 + rotation * new Vector3(Thickness / 2.0f, 0f);
        vh.AddVert(vertex);

        vertex.position = point2 + rotation * new Vector3(-Thickness / 2.0f, 0f);
        vh.AddVert(vertex);
        vertex.position = point2 + rotation * new Vector3(Thickness / 2.0f, 0f);
        vh.AddVert(vertex);

        vertex.position = point2;
        vh.AddVert(vertex);
    }
    
    private float GetAngleTowardTarget(Vector2 vertex, Vector2 target)
    {
        return Mathf.Atan2(target.y - vertex.y, target.x - vertex.x) * (180 / Mathf.PI);
    }
}