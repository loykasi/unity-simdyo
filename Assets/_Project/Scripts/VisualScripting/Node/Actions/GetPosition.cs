using UnityEngine;

[CreateAssetMenu(fileName = "GetPosition", menuName = "Scriptable Objects/Visual Scripting/Node/Get Position")]
public class GetPosition : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new GetPositionNode(Title);
    }
}

class GetPositionNode : ScriptNode
{
    public OutputValue Value;

    public GetPositionNode(string title) : base(title)
    {
        Value = OutputValue(
            nameof(Value),
            (vs) =>
            {
                return vs.Entity.transform.position;
            }
        );
    }
}