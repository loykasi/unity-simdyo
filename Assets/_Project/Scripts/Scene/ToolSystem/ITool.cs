using UnityEngine;

public interface ITool
{
    ToolType Type { get; }
    void Enable();
    void Disable();
    void OnUpdate();
}