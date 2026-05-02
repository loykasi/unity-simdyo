using UnityEngine;
using UnityEngine.InputSystem;

public class TracerTool : PanTool
{
    public override ToolType Type => ToolType.Tracer;

    private Vector3 _startPosition;

    public override void OnUpdate()
    {
        Zoom();
        HandleContextMenu();
        HandlePanRightMouse();
        Create();
        HandleSelection();
    }

    private void Create()
    {
        Vector3 mousePosition = GetMouseWorldPositon();
        if (Mouse.current.leftButton.wasPressedThisFrame && !ScreenInteractionUtils.IsOverUI())
        {
            _startPosition = Vector3Utils.GetGridPosition(mousePosition);
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame && !ScreenInteractionUtils.IsOverUI())
        {
            if (!ObjectManager.Instance.TryGetSceneEntity(_startPosition, out SceneEntity onHoveredEntity))
            {
                return;
            }
            
            if (onHoveredEntity is TracerEntity)
            {
                return;
            }
            
            TracerEntity entity = ObjectManager.Instance.AddTracer(_startPosition);
            entity.AutoAttachToMeshEntity();
        }
    }
}