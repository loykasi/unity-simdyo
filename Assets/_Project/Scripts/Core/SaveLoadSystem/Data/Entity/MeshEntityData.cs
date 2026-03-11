using UnityEngine;

[System.Serializable]
public abstract class MeshEntityData : EntityData
{
    public bool ColliderEnabled;
    public bool GravityEnabled;
    public CollisionLayer Layer;
    public ColorHSV Color;
    public string TextureSlotKey;
}