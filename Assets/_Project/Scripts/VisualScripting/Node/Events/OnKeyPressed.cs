using UnityEngine;

[CreateAssetMenu(fileName = "OnKeyPressedNode", menuName = "Scriptable Objects/Visual Scripting/Node/KeyPressed")]
public class OnKeyPressed : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new OnKeyPressedNode(Title);
    }
}

public class OnKeyPressedNode : EventNode
{
    public OnKeyPressedNode(string title) : base(title)
    {
    }

    public override EventHook Hook => EventHook.Start;
}