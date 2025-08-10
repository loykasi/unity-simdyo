using UnityEngine;

public static class Vector3Utils
{
    public static Vector3 ProjectOnVector(Vector3 point, Vector3 startPoint, Vector3 direction)
    {
        Vector3 vec = point - startPoint;
        float dot = Vector3.Dot(direction, vec);
        return startPoint + dot * direction;
    }

    public static Vector3 RotatePointAroundPoint(Vector3 point, Vector3 center, Quaternion rotation)
    {
        return rotation * (point - center) + center;
    }

    public static Vector3 GetGridPosition(Vector3 position)
    {
        return GridController.Instance.GetPosition(position);
    }
}