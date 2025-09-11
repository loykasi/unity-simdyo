using UnityEngine;

[CreateAssetMenu(fileName = "SetVariable", menuName = "Scriptable Objects/Visual Scripting/Node/Set Variable")]
public class SetVariable : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new SetVariableNode(Title);
    }
}

public class SetVariableNode : ScriptNode
{
    public InputTrigger Enter;
    public OutputTrigger Exit;

    public InputValue Variable;
    public InputValue Value;

    public SetVariableNode(string title) : base(title)
    {
        Enter = InputTrigger(nameof(Enter), Set);
        Exit = OutputTrigger(nameof(Exit));
        Variable = InputValue(nameof(Variable));
        Value = InputValue(nameof(Value));
    }

    private OutputTrigger Set(ScriptFlow vs)
    {
        string name = Variable.GetValue(vs).ToString();
        object value = Value.GetValue(vs);
        vs.UpdateVariable(name, value);
        return Exit;
    }
}