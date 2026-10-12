using UnityEngine;

public class TracerTool : BaseTool
{
    public override ToolType Type => ToolType.Tracer;
    public override bool IsAllowSelectOnClick => false;

    private Vector2 _startPosition;

    public override bool OnPointerLeftDown(Vector2 position)
    {
        _startPosition = position;
        return true;
    }

    public override void OnPointerLeftUp(Vector2 position)
    {        
        if (_startPosition != position)
        {
            return;
        }

        Vector3 worldPosition = Utils.ToWorldPositon(_startPosition);
        if (ObjectManager.Instance.TryGetSceneEntity(worldPosition, out SceneEntity onHoveredEntity)
            && onHoveredEntity is TracerEntity)
        {
            return;
        }
        
        TracerEntity entity = ObjectManager.Instance.AddTracer(worldPosition);
        entity.AutoAttachToMeshEntity();
    }
}