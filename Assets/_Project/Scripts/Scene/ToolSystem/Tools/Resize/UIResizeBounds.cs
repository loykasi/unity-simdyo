using UnityEngine;

public class UIResizeBounds : MonoBehaviour
{
    private ResizeTool _resizeTool;

    private void Start()
    {
        _resizeTool = ToolManager.Instance.GetTool(ToolType.Resize) as ResizeTool;
    }

    public void BeginDrag(BoundsHandleDirection direction)
    {
        _resizeTool?.BeginResize(direction);
    }
    
    public void Drag(BoundsHandleDirection direction, Vector3 position)
    {
        _resizeTool?.Resize(direction, position);
    }
}