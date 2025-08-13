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
    public InputTrigger Enter;

    public BreakNode(string title) : base(title)
    {
        Enter = InputTrigger(
            nameof(Enter),
            (vs) =>
            {
                vs.BreakLoop();
                
                return null;
            }
        );
    }
}