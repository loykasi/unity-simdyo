public class MoveTool : BaseTool
{
    public override ToolType Type => ToolType.Move;

    private InteractionMove _move = new();

    public override void Enable()
    {
        base.Enable();
        _move.Enable();
    }

    public override void Disable()
    {
        base.Disable();
        _move.Disable();
    }
}