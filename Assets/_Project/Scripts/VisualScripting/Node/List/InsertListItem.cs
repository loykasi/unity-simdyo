using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "InsertItem", menuName = "Scriptable Objects/Visual Scripting/Node/List/Insert")]
public class InsertListItem : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new InsertListItemNode(Title);
    }
}

class InsertListItemNode : ScriptNode
{
    public InputTrigger Enter;
    public OutputTrigger Exit;

    public ValueInput ListInput;
    public ValueInput ItemInput;
    public ValueInput IndexInput;
    public ValueOutput Output;

    public InsertListItemNode(string title) : base(title)
    {
        Enter = CreateInputTrigger(Set);
        Exit = CreateOutputTrigger();

        ListInput = ValueInput(DataType.List, false);
        ItemInput = ValueInput(true);
        IndexInput = ValueInput(true);
        Output = ValueOutput(DataType.List, Get);
    }

    private OutputTrigger Set(VisualScripting vs)
    {
        IList list = (IList)ListInput.GetValue(vs);
        object item = ItemInput.GetValue(vs);
        int index = (int)(float)IndexInput.GetValue(vs);
        list.Insert(index, item);
        return Exit;
    }

    private object Get(VisualScripting vs)
    {
        IList list = (IList)ListInput.GetValue(vs);
        return list;
    }
}