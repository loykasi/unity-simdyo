using UnityEngine;

public interface ITool
{
    ToolType Type { get; }
    bool IsAllowSelectOnClick { get; }

    void Enable();
    void Disable();
    void OnUpdate();

    bool OnPointerLeftDown(Vector2 position);
    void OnPointerLeftUp(Vector2 position);
    bool OnPointerRightDown(Vector2 position);
    void OnPointerRightUp(Vector2 position);
    void OnPointerMove(Vector2 position);
    bool OnScroll(float value);
}