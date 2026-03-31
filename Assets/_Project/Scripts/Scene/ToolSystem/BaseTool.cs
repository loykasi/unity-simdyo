using UnityEngine;

public abstract class BaseTool : MonoBehaviour, ITool
{
    public abstract ToolType Type { get; }

    public abstract void Enable();
    public abstract void Disable();
    public abstract void OnUpdate();
}