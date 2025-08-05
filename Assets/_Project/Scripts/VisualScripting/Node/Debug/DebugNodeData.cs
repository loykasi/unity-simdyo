using System.Collections;
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
            var value = Value.GetValue(vs);
            LogCommand.Instance.Log(value);

            if (value is IList list)
            {
                string listValue = "";
                foreach (var item in list)
                {
                    listValue += item.ToString() + " | ";
                }
                Debug.Log($"list: {listValue}");
            }
            else
            {
                Debug.Log(value);
            }
            
            return outputTrigger;
        });
        outputTrigger = CreateOutputTrigger();
        Value = ValueInput();
    }
}