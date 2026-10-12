using UnityEngine;

public class MoveTool : BaseTool
{
    public override ToolType Type => ToolType.Move;
    private readonly InteractionMove _move = new();

    public override void Disable()
    {
        _move.End();
    }

    public override bool OnPointerLeftDown(Vector2 position)
    {
        _move.Begin();
        return true;
    }

    public override void OnPointerLeftUp(Vector2 position)
    {
        _move.End();
    }

    public override void OnPointerMove(Vector2 position)
    {
        _move.Update(position);
    }
}