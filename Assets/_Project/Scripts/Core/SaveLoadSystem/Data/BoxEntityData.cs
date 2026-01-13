using TMPro;

[System.Serializable]
public class BoxEntityData : EntityData
{
    public float Width;
    public float Height;

    public string Text;
    public ColorHSV TextColor;
    public float TextSize;
    public HorizontalAlignmentOptions TextHorizontalAlignment;
    public VerticalAlignmentOptions TextVerticalAlignment;
}