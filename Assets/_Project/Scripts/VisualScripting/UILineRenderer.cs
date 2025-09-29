using UnityEngine;
using UnityEngine.UI;

/// NEED REFACTOR
namespace Loykas.Scripting
{
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
            float line1Dist = (point2 - point1).sqrMagnitude;
            float line2Dist = (point2 - point3).sqrMagnitude;

            Vector3 dir1 = (point2 - point1).normalized;
            Vector3 dir2 = (point3 - point2).normalized;

            float angle = Vector3.Angle(dir1, dir2);
            float angleDelta = Mathf.Min(angle / 90.0f, 1.0f) * Mathf.Min((180.0f - angle) / 90.0f, 1.0f);
            float distDelta = Mathf.Min(line1Dist / (CornerRadius * CornerRadius), 1.0f) * Mathf.Min(line2Dist / (CornerRadius * CornerRadius), 1.0f);
            float radius = Mathf.Lerp(0, CornerRadius, angleDelta * distDelta);

            // Debug.Log(radius);

            float dot = Vector3.Dot(new Vector3(dir1.y, -dir1.x), dir2);

            Vector3 normal1 = dot >= 0 ? new(dir1.y, -dir1.x, 0f) : new(-dir1.y, dir1.x, 0f);
            Vector3 normal2 = dot >= 0 ? new(dir2.y, -dir2.x, 0f) : new(-dir2.y, dir2.x, 0f);

            Vector3 a = point1 + normal1 * radius;
            Vector3 b = point2 + normal1 * radius;
            Vector3 c = point2 + normal2 * radius;
            Vector3 d = point3 + normal2 * radius;

            Vector3 v1 = b - a;
            Vector3 v2 = d - c;

            float den = v1.x * v2.y - v1.y * v2.x;
            float k = den == 0 ? 0 : (v2.y * (c - a).x - v2.x * (c - a).y) / den;

            Vector3 center = a + k * v1;
            tangent1 = center - normal1 * radius;
            tangent2 = center - normal2 * radius;

            float tangent1Dist = (point2 - tangent1).sqrMagnitude;
            float tangent2Dist = (point2 - tangent2).sqrMagnitude;

            if (line1Dist < tangent1Dist)
            {
                Vector3 offset = point1 - tangent1;
                center += offset;
                tangent1 += offset;
                tangent2 += offset;
            }
            else if (line2Dist < tangent2Dist)
            {
                Vector3 offset = point3 - tangent2;
                center += offset;
                tangent1 += offset;
                tangent2 += offset;
            }

            return center;
        }

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

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
            if (Points == null || Points.Length < 2)
                return;

            for (int i = 0; i < Points.Length; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(Points[i] + transform.position, 15f);
            }

            for (int i = 0; i < Points.Length - 3; i++)
            {
                Vector3 center = GetCircleCenter(Points[i], Points[i + 1], Points[i + 2], out Vector3 tangent1, out Vector3 tangent2);
                // Gizmos.color = Color.blue;
                // Gizmos.DrawWireSphere(center + transform.position, CornerRadius);

                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(tangent1 + transform.position, 10f);
                Gizmos.DrawWireSphere(tangent2 + transform.position, 10f);


                Vector3 v1 = tangent1 - center;
                Vector3 v2 = tangent2 - center;
                float angle = Vector3.Angle(v1, v2);
                float step = angle / CornerSegment;
                for (int j = 1; j <= CornerSegment - 1; j++)
                {
                    Vector3 point = Quaternion.AngleAxis(step * j, Vector3.back) * v1;
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawWireSphere(point + center + transform.position, 5f);
                }
            }
        }
    }
}