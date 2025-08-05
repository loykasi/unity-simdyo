using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ListContainItem", menuName = "Scriptable Objects/Visual Scripting/Node/List/Contain")]
public class ListContainsItem : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new ListContainsItemNode(Title);
    }
}

class ListContainsItemNode : ScriptNode
{
    public ValueInput ListInput;
    public ValueInput ItemInput;
    public ValueOutput Output;

    public ListContainsItemNode(string title) : base(title)
    {
        ListInput = ValueInput(DataType.List, false);
        ItemInput = ValueInput(true);
        Output = ValueOutput(DataType.Boolean, Get);
    }

    private object Get(VisualScripting vs)
    {
        IList list = (IList)ListInput.GetValue(vs);
        object item = ItemInput.GetValue(vs);
        return list.Contains(item);
    }
}