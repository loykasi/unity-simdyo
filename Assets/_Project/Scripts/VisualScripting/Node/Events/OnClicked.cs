using UnityEngine;

[CreateAssetMenu(fileName = "OnClickedNode", menuName = "Scriptable Objects/Visual Scripting/Node/Clicked")]
public class OnClicked : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new OnClickedNode(Title);
    }
}

public class OnClickedNode : EventNode
{
    public OnClickedNode(string title) : base(title)
    {
    }

    public override EventHook Hook => EventHook.Clicked;
}