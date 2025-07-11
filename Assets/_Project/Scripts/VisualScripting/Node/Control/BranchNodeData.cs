using UnityEngine;

[CreateAssetMenu(fileName = "Branch", menuName = "Scriptable Objects/Visual Scripting/Node/Branch")]
public class BranchNodeData : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new BranchNode(Title);
    }
}

class BranchNode : ScriptNode
{
    public InputTrigger InputTrigger;
    public OutputTrigger OutputTriggerA;
    public OutputTrigger OutputTriggerB;

    public ValueInput Value;

    public BranchNode(string title): base(title)
    {
        InputTrigger = CreateInputTrigger(() =>
        {
            if ((bool)Value.GetValue())
            {
                return OutputTriggerA;
            }
            return OutputTriggerB;
        });
        OutputTriggerA = CreateOutputTrigger();
        OutputTriggerB = CreateOutputTrigger();
        Value = ValueInput();
    }
}