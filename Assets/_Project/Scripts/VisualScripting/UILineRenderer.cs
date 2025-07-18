using UnityEngine;
using UnityEngine.UI;

/// NEED REFACTOR
public class UILineRenderer : MaskableGraphic, ICanvasRaycastFilter
{
    public RectTransform Rect;
    public Vector3[] Points;
    public float Thickness;
    public float CornerRadius;
    public int CornerSegment;

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

        if (Points == null || Points.Length < 3)
            return;

        Vector3 point = Points[0];
        for (int i = 0; i < Points.Length - 1; i++)
        {
            if (i == Points.Length - 2)
            {
                DrawLine(point, Points[i + 1], vh);
                int index;
                if (CornerRadius <= 0 || CornerSegment <= 0)
                {
                    index = i * 4;
                }
                else
                {
                    index = i * (4 + (CornerSegment - 1) * 2);
                }
                vh.AddTriangle(index + 0, index + 3, index + 1);
                vh.AddTriangle(index + 3, index + 0, index + 2);
                if (i != 0)
                {
                    vh.AddTriangle(index - 0, index - 1, index - 2);
                    vh.AddTriangle(index + 0, index + 1, index - 1);
                }
                break;
            }

            point = DrawSegment(point, Points[i + 1], Points[i + 2], i, vh);
        }
    }

    private Vector3 DrawSegment(Vector3 point1, Vector3 point2, Vector3 point3, int pointIndex, VertexHelper vh)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        if (CornerRadius <= 0 || CornerSegment <= 0)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, GetAngleTowardTarget(point1, point2) - 90f);
            vertex.position = point1 + rotation * new Vector3(-Thickness / 2.0f, 0f);
            vh.AddVert(vertex);
            vertex.position = point1 + rotation * new Vector3(Thickness / 2.0f, 0f);
            vh.AddVert(vertex);

            vertex.position = point2 + rotation * new Vector3(-Thickness / 2.0f, 0f);
            vh.AddVert(vertex);
            vertex.position = point2 + rotation * new Vector3(Thickness / 2.0f, 0f);
            vh.AddVert(vertex);

            int index = pointIndex * 4;

            if (index != 0)
            {
                vh.AddTriangle(index - 0, index - 1, index - 2);
                vh.AddTriangle(index + 0, index + 1, index - 1);
            }

            vh.AddTriangle(index + 0, index + 3, index + 1);
            vh.AddTriangle(index + 3, index + 0, index + 2);

            return point2;
        }
        else
        {
            Vector3 center = GetCircleCenter(point1, point2, point3, out Vector3 tangent1, out Vector3 tangent2);
            Quaternion rotation = Quaternion.Euler(0, 0, GetAngleTowardTarget(point1, tangent1) - 90f);

            vertex.position = point1 + rotation * new Vector3(-Thickness / 2.0f, 0f);
            vh.AddVert(vertex);
            vertex.position = point1 + rotation * new Vector3(Thickness / 2.0f, 0f);
            vh.AddVert(vertex);

            vertex.position = tangent1 + rotation * new Vector3(-Thickness / 2.0f, 0f);
            vh.AddVert(vertex);
            vertex.position = tangent1 + rotation * new Vector3(Thickness / 2.0f, 0f);
            vh.AddVert(vertex);

            int index = pointIndex * (4 + (CornerSegment - 1) * 2);

            if (index != 0)
            {
                vh.AddTriangle(index - 0, index - 1, index - 2);
                vh.AddTriangle(index + 0, index + 1, index - 1);
            }

            vh.AddTriangle(index + 0, index + 3, index + 1);
            vh.AddTriangle(index + 3, index + 0, index + 2);

            Vector3 v1 = tangent1 - center;
            Vector3 v2 = tangent2 - center;
            float angle = Vector3.Angle(v1, v2);
            float step = angle / CornerSegment;
            index += 2;
            for (int i = 1; i <= CornerSegment - 1; i++)
            {
                Vector3 cross = Vector3.Cross(v1, v2);
                Vector3 v = Quaternion.AngleAxis(step * i, cross) * v1;
                Vector3 point = v + center;

                Quaternion rot = Quaternion.FromToRotation(Vector3.left, cross.z < 0 ? v : -v);

                vertex.position = point + rot * new Vector3(-Thickness / 2.0f, 0f);
                vh.AddVert(vertex);
                vertex.position = point + rot * new Vector3(Thickness / 2.0f, 0f);
                vh.AddVert(vertex);

                vh.AddTriangle(index + 0, index + 3, index + 1);
                vh.AddTriangle(index + 3, index + 0, index + 2);
                index += 2;
            }

            return tangent2;
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
    }

    private float GetAngleTowardTarget(Vector2 vertex, Vector2 target)
    {
        return Mathf.Atan2(target.y - vertex.y, target.x - vertex.x) * (180 / Mathf.PI);
    }

    private Vector3 GetCircleCenter(Vector3 point1, Vector3 point2, Vector3 point3, out Vector3 tangent1, out Vector3 tangent2)
    {
        Vector3 dir1 = (point2 - point1).normalized;
        Vector3 dir2 = (point3 - point2).normalized;

        float dot = Vector3.Dot(new Vector3(dir1.y, -dir1.x), dir2);

        Vector3 normal1 = dot >= 0 ? new(dir1.y, -dir1.x, 0f) : new(-dir1.y, dir1.x, 0f);
        Vector3 normal2 = dot >= 0 ? new(dir2.y, -dir2.x, 0f) : new(-dir2.y, dir2.x, 0f);

        Vector3 a = point1 + normal1 * CornerRadius;
        Vector3 b = point2 + normal1 * CornerRadius;
        Vector3 c = point2 + normal2 * CornerRadius;
        Vector3 d = point3 + normal2 * CornerRadius;

        Vector3 v1 = b - a;
        Vector3 v2 = d - c;

        float den = v1.x * v2.y - v1.y * v2.x;
        float k = (v2.y * (c - a).x - v2.x * (c - a).y) / den;

        Vector3 center = a + k * v1;
        tangent1 = center - normal1 * CornerRadius;
        tangent2 = center - normal2 * CornerRadius;

        return center;
    }

    // public override bool Raycast(Vector2 screenPoint, Camera eventCamera)
    // {
    //     Vector3 lineDir = (Points[2] - Points[1]).normalized;
    //     Vector3 v = (Vector3)screenPoint - Points[2];

    //     float delta = Vector3.Dot(v, lineDir);
    //     Vector3 projectPoint = Points[2] + lineDir * delta;

    //     float dist = ((Vector3)screenPoint - projectPoint).sqrMagnitude;
    //     return dist < 160000;
    // }

    public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        Vector3 point1 = Points[1] + Rect.position;
        Vector3 point2 = Points[2] + Rect.position;

        Vector3 lineDir = (point2 - point1).normalized;
        Vector3 v = (Vector3)screenPoint - point1;
        float delta = Vector3.Dot(v, lineDir);

        Vector3 projectPoint = point1 + lineDir * delta;

        float dist = ((Vector3)screenPoint - projectPoint).sqrMagnitude;
        return dist < 450;
    }

    // public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    // {
    //     float dist1 = (Points[0] + transform.position - new Vector3(screenPoint.x, screenPoint.y)).sqrMagnitude;
    //     float dist2 = (Points[3] + transform.position - new Vector3(screenPoint.x, screenPoint.y)).sqrMagnitude;
    //     return dist2 < 300 || dist1 < 300;
    // }

    // private void OnDrawGizmos()
    // {
    //     if (Points == null || Points.Length < 2)
    //         return;

    //     for (int i = 0; i < Points.Length; i++)
    //     {
    //         Gizmos.color = Color.red;
    //         Gizmos.DrawWireSphere(Points[i] + transform.position, 15f);
    //     }

    //     Vector3 center = GetCircleCenter(Points[0], Points[1], Points[2], out Vector3 tangent1, out Vector3 tangent2);
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawWireSphere(center + transform.position, CornerRadius);

    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireSphere(tangent1 + transform.position, 10f);
    //     Gizmos.DrawWireSphere(tangent2 + transform.position, 10f);



    //     Vector3 v1 = tangent1 - center;
    //     Vector3 v2 = tangent2 - center;
    //     float angle = Vector3.Angle(v1, v2);
    //     float step = angle / CornerSegment;
    //     for (int i = 1; i <= CornerSegment - 1; i++)
    //     {
    //         Vector3 point = Quaternion.AngleAxis(step * i, Vector3.back) * v1;
    //         Gizmos.color = Color.cyan;
    //         Gizmos.DrawWireSphere(point + center + transform.position, 5f);
    //     }
    // }
}