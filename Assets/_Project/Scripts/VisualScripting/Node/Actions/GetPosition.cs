using Unity.VisualScripting;
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
    public InputValue Input;
    public OutputValue Value;

    public GetPositionNode(string title) : base(title)
    {
        Input = InputValue(nameof(Input), DataType.Entity).UseInput();
        Value = OutputValue(
            nameof(Value),
            GetPosition
        );
    }

    private object GetPosition(ScriptFlow vs)
    {
        if (Input.InputType == InputValueTypes.Entity && Input.Value != null)
        {
            SceneEntity entity = Input.Value as SceneEntity;
            return entity.Position;
        }
        
        return vs.Entity.Position;
    }
}