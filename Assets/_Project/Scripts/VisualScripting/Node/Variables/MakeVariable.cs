using UnityEngine;

// [CreateAssetMenu(fileName = "Break", menuName = "Scriptable Objects/Visual Scripting/Node/Break")]
public abstract class MakeVariable<T> : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new MakeVariableNode<T>(Title);
    }
}

public class MakeVariableNode<T> : ScriptNode
{
    public ValueInput input;
    public ValueOutput output;

    public MakeVariableNode(string title) : base(title)
    {
        input = ValueInput<T>(true);
        output = ValueOutput(() => (T)input.GetValue());
    }
}