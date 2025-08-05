using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "RemoveItem", menuName = "Scriptable Objects/Visual Scripting/Node/List/Remove Item")]
public class RemoveListItem : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new RemoveListItemNode(Title);
    }
}

class RemoveListItemNode : ScriptNode
{
    public InputTrigger Enter;
    public OutputTrigger Exit;

    public ValueInput ListInput;
    public ValueInput IndexInput;
    public ValueOutput Output;

    public RemoveListItemNode(string title) : base(title)
    {
        Enter = CreateInputTrigger(Set);
        Exit = CreateOutputTrigger();

        ListInput = ValueInput(DataType.List, false);
        IndexInput = ValueInput(true);
        Output = ValueOutput(DataType.List, Get);
    }

    private OutputTrigger Set(VisualScripting vs)
    {
        IList list = (IList)ListInput.GetValue(vs);
        int index = (int)(float)IndexInput.GetValue(vs);
        list.RemoveAt(index);
        return Exit;
    }

    private object Get(VisualScripting vs)
    {
        IList list = (IList)ListInput.GetValue(vs);
        return list;
    }
}