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
    public InputTrigger InputTrigger;
    public OutputTrigger OuputTrigger;

    public ValueInput inputVariable;
    public ValueInput inputValue;

    public SetVariableNode(string title) : base(title)
    {
        InputTrigger = CreateInputTrigger(Set);
        OuputTrigger = CreateOutputTrigger();
        inputVariable = ValueInput(true);
        inputValue = ValueInput(true);
    }

    private OutputTrigger Set(VisualScripting vs)
    {
        string name = inputVariable.GetValue(vs).ToString();
        object value = inputValue.GetValue(vs);
        vs.UpdateVariable(name, value);
        return OuputTrigger;
    }
}