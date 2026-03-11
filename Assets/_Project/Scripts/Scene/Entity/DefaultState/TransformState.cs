using UnityEngine;

public struct TransformState
{
    public Vector3 Position;
    public Quaternion Rotation;
    public int ZDepth;

    public TransformState(Vector3 position, Quaternion rotation, int zDepth)
    {
        Position = position;
        Rotation = rotation;
        ZDepth = zDepth;
    }
}