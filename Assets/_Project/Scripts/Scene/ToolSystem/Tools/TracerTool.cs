using UnityEngine;

public class TracerTool : BaseTool
{
    public override ToolType Type => ToolType.Tracer;

    private Vector2 _startPosition;

    protected override void OnClick()
    {
        if (ScreenInteractionUtils.IsOverUI())
        {
            return;
        }

        _startPosition = InputManager.Instance.MousePosition;
    }

    protected override void OnClickReleased()
    {
        if (ScreenInteractionUtils.IsOverUI())
        {
            return;
        }
        
        if (_startPosition != InputManager.Instance.MousePosition)
        {
            return;
        }

        if (!ObjectManager.Instance.TryGetSceneEntity(_startPosition, out SceneEntity onHoveredEntity)
            && onHoveredEntity is TracerEntity)
        {
            return;
        }
        
        Vector3 position = Utils.ToWorldPositon(_startPosition);
        TracerEntity entity = ObjectManager.Instance.AddTracer(position);
        entity.AutoAttachToMeshEntity();
    }
}