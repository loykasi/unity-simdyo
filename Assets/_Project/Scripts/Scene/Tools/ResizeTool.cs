using UnityEngine;
using UnityEngine.InputSystem;

public class ResizeTool : ITool
{
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