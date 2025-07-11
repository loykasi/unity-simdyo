using UnityEngine;

[CreateAssetMenu(fileName = "StartNode", menuName = "Scriptable Objects/Visual Scripting/Node/Start")]
public class StartNodeData : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new StartNode(Title);
    }
}

public class StartNode : EventNode
{
    public StartNode(string title) : base(title)
    {
    }

    public override EventHook GetHook()
    {
        return EventHook.Start;
    }
}