using UnityEngine;

[CreateAssetMenu(fileName = "LogNode", menuName = "Scriptable Objects/Visual Scripting/Node/Log")]
public class DebugNodeData : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new DebugNode(Title);
    }
}

class DebugNode : ScriptNode
{
    public InputTrigger inputTrigger;
    public OutputTrigger outputTrigger;
    public ValueInput Value;

    public DebugNode(string title) : base(title)
    {
        inputTrigger = CreateInputTrigger((vs) =>
        {
            LogCommand.Instance.Log(Value.GetValue());
            Debug.Log(Value.GetValue());
            return outputTrigger;
        });
        outputTrigger = CreateOutputTrigger();
        Value = ValueInput();
    }
}