public class ResizeTool : PanTool
{
    public override ToolType Type => ToolType.Resize;

    public override void Enable()
    {
        ResizeController.Instance.Enable();
    }

    public override void Disable()
    {
        ResizeController.Instance.Disable();
    }
}