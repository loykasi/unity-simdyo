public class PanTool : BaseTool
{
    public override ToolType Type => ToolType.Pan;

    protected override void OnClick()
    {
        _pan.Start();
    }

    protected override void OnClickReleased()
    {
        _pan.Stop();
    }
}