using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "AddItem", menuName = "Scriptable Objects/Visual Scripting/Node/List/Add Item")]
public class AddListItem : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new AddListItemNode(Title);
    }
}

class AddListItemNode : ScriptNode
{
    public InputTrigger Enter;
    public OutputTrigger Exit;

    public ValueInput ListInput;
    public ValueInput ItemInput;
    public ValueOutput Output;

    public AddListItemNode(string title) : base(title)
    {
        Enter = CreateInputTrigger(Set);
        Exit = CreateOutputTrigger();

        ListInput = ValueInput(DataType.List, false);
        ItemInput = ValueInput(true);
        Output = ValueOutput(DataType.List, Get);
    }

    private OutputTrigger Set(VisualScripting vs)
    {
        IList list = (IList)ListInput.GetValue(vs);
        object item = ItemInput.GetValue(vs);
        list.Add(item);
        return Exit;
    }

    private object Get(VisualScripting vs)
    {
        IList list = (IList)ListInput.GetValue(vs);
        return list;
    }
}