using TMPro;

[System.Serializable]
public class BoxEntityData : MeshEntityData
{
    public float Width;
    public float Height;

    public string Text;
    public ColorHSV TextColor;
    public float TextSize;
    public HorizontalAlignmentOptions TextHorizontalAlignment;
    public VerticalAlignmentOptions TextVerticalAlignment;

    public override SceneEntity CreateEntity()
    {
        BoxEntity entity = ObjectManager.Instance.AddBox(Position, Width, Height);

        entity.Id = Id;
        entity.Name = Name;
        entity.Rotation = Rotation;
        entity.CurrentColor = Color;
        entity.IsColliderEnabled = ColliderEnabled;
        entity.IsGravityEnabled = GravityEnabled;
        entity.SetLayer(Layer);
        entity.SetTexture(TextureSlotKey);
        entity.ZDepth = ZDepth;

        entity.TextBox.Text = Text;
        entity.TextBox.Color = TextColor.ToUnityColor();
        entity.TextBox.Size = TextSize;
        entity.TextBox.HorizontalAlignment = TextHorizontalAlignment;
        entity.TextBox.VerticalAlignment = TextVerticalAlignment;

        return entity;
    }
}