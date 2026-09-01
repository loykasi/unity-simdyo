using UnityEngine;

public class UIResizeBounds : MonoBehaviour
{
    private ResizeTool _resizeTool;

    private void Start()
    {
        _resizeTool = (ResizeTool)ToolManager.Instance.GetTool(ToolType.Resize);
    }

    public void BeginDrag(BoundsHandleDirection direction)
    {
        _resizeTool.BeginResize(direction);
    }
    
    public void Drag(BoundsHandleDirection direction, Vector3 position)
    {
        _resizeTool.Resize(direction, position);
    }
}