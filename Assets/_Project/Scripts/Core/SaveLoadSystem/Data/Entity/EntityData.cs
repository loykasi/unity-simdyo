using UnityEngine;

[System.Serializable]
public abstract class EntityData
{
    public int Id;
    public string Name;
    public EntityType Type;
    public Vector3 Position;
    public Quaternion Rotation;
    public int ZDepth;
    public ScriptFlowData Script = new();

    public abstract SceneEntity CreateEntity();
}