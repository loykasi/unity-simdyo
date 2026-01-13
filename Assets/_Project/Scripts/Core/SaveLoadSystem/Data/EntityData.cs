using UnityEngine;

[System.Serializable]
public class EntityData
{
    public int Id;
    public string Name;
    public EntityType Type;
    public Vector3 Position;
    public Quaternion Rotation;
    public bool ColliderEnabled;
    public bool GravityEnabled;
    public CollisionLayer Layer;
    public ColorHSV Color;
    public string TextureSlotKey;
    public int ZDepth;
    public ScriptFlowData Script = new();
}