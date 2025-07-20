using UnityEngine;

[CreateAssetMenu(fileName = "Update", menuName = "Scriptable Objects/Visual Scripting/Node/Update")]
public class Update : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new UpdateNode(Title);
    }
}

public class UpdateNode : EventNode
{
    public UpdateNode(string title) : base(title)
    {
    }

    public override EventHook GetHook()
    {
        return EventHook.Update;
    }
}