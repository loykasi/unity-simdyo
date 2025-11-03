using UnityEngine;

[System.Serializable]
public class EntityData
{
    public EntityType Type;
    public Vector3 Position;
    public Quaternion Rotation;
    public bool ColliderEnabled;
    public bool GravityEnabled;
    public CollisionLayer Layer;
    public ColorHSV Color;
    public int TextureSlot;
    public ScriptFlowData Script = new();
}