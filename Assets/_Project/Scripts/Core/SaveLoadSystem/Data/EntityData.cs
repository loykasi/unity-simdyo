using UnityEngine;

[System.Serializable]
public class EntityData
{
    public EntityType Type;
    public Vector3 Position;
    public Quaternion Rotation;
    public ColorHSV Color;
    public ScriptElementData Script = new();
}