using UnityEngine;
using UnityEngine.InputSystem;

public class ResizeTool : ITool
{
    public ToolType Type => ToolType.Resize;

    public void Enable()
    {
        ResizeController.Instance.Enable();
    }

    public void Disable()
    {
        ResizeController.Instance.Disable();
    }

    public void OnUpdate(Vector3 mousePosition)
    {

    }
}