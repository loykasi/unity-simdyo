using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ClearList", menuName = "Scriptable Objects/Visual Scripting/Node/List/Clear")]
public class ClearList : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new ClearListNode(Title);
    }
}

class ClearListNode : ScriptNode
{
    public InputTrigger Enter;
    public OutputTrigger Exit;

    public ValueInput ListInput;

    public ClearListNode(string title) : base(title)
    {
        Enter = CreateInputTrigger(Clear);
        Exit = CreateOutputTrigger();

        ListInput = ValueInput(DataType.List, false);
    }

    private OutputTrigger Clear(VisualScripting vs)
    {
        IList list = (IList)ListInput.GetValue(vs);
        list.Clear();
        return Exit;
    }
}