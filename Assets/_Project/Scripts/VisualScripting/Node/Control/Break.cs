using UnityEngine;

[CreateAssetMenu(fileName = "Break", menuName = "Scriptable Objects/Visual Scripting/Node/Break")]
public class Break : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new BreakNode(Title);
    }
}

class BreakNode : ScriptNode
{
    public InputTrigger InputTrigger;

    public BreakNode(string title) : base(title)
    {
        InputTrigger = CreateInputTrigger((vs) =>
        {
            vs.BreakLoop();
            
            return null;
        });
    }
}