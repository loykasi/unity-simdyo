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
    public InputValue ListInput;
    public InputValue Item;
    public OutputValue Output;

    public ListContainsItemNode(string title) : base(title)
    {
        ListInput = InputValue(nameof(ListInput), DataType.List, false);
        Item = InputValue(nameof(Item), true);
        Output = OutputValue(nameof(Output), DataType.Boolean, Get);
    }

    private object Get(VisualScripting vs)
    {
        IList list = (IList)ListInput.GetValue(vs);
        object item = Item.GetValue(vs);
        return list.Contains(item);
    }
}