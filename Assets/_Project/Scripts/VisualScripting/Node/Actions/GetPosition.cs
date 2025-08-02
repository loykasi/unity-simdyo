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
    public ValueOutput Output;

    public GetPositionNode(string title) : base(title)
    {
        Output = ValueOutput((vs) =>
        {
            return vs.Entity.transform.position;
        });
    }
}