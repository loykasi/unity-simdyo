using System;
using UnityEngine;

public abstract class BaseTool : ITool
{
    public abstract ToolType Type { get; }
    public virtual bool IsAllowSelectOnClick => true;


    public virtual void Enable() { }
    public virtual void Disable() { }
    public virtual void OnUpdate() { }

    public virtual bool OnPointerLeftDown(Vector2 position) => false;
    public virtual void OnPointerLeftUp(Vector2 position) { }
    public virtual bool OnPointerRightDown(Vector2 position) => false;
    public virtual void OnPointerRightUp(Vector2 position) { }
    public virtual bool OnScroll(float value) => false;
    public virtual void OnPointerMove(Vector2 position) { }
}