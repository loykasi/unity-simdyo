using UnityEngine;

[System.Serializable]
public class PolygonEntityData : MeshEntityData
{
    public Vector2[] PolygonPoints;

    public override SceneEntity CreateEntity()
    {
        PolygonEntity entity = ObjectManager.Instance.AddPolygon(Position, PolygonPoints);

        entity.Id = Id;
        entity.Name = Name;
        entity.Rotation = Rotation;
        entity.CurrentColor = Color;
        entity.IsColliderEnabled = ColliderEnabled;
        entity.IsGravityEnabled = GravityEnabled;
        entity.SetLayer(Layer);
        entity.SetTexture(TextureSlotKey);
        entity.ZDepth = ZDepth;

        return entity;
    }
}