public class TracerEntityData : EntityData
{
    public float Time;
    public float Diameter;
    public ColorHSV Color;
    public int ParentId;

    public override SceneEntity CreateEntity()
    {
        TracerEntity entity = ObjectManager.Instance.AddTracer(Position);

        entity.Id = Id;
        entity.Name = Name;
        entity.Rotation = Rotation;
        entity.ZDepth = ZDepth;
        
        entity.Time = Time;
        entity.Diameter = Diameter;
        entity.Color = Color;
        entity.ParentId = ParentId;

        return entity;
    }
}