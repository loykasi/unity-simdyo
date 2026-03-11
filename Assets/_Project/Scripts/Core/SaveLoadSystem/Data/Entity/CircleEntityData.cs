[System.Serializable]
public class CircleEntityData : MeshEntityData
{
    public float Radius;

    public override SceneEntity CreateEntity()
    {
        CircleEntity entity = ObjectManager.Instance.AddCircle(Position, Radius);

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