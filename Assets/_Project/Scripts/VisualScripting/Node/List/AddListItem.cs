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

    public InputValue ListInput;
    public InputValue Item;
    public OutputValue Output;

    public AddListItemNode(string title) : base(title)
    {
        Enter = InputTrigger(nameof(Enter), Set);
        Exit = OutputTrigger(nameof(Exit));

        ListInput = InputValue(nameof(ListInput), DataType.List, false);
        Item = InputValue(nameof(Item), true);
        Output = OutputValue(nameof(Output), DataType.List, Get);
    }

    private OutputTrigger Set(VisualScripting vs)
    {
        IList list = (IList)ListInput.GetValue(vs);
        object item = Item.GetValue(vs);
        list.Add(item);
        return Exit;
    }

    private object Get(VisualScripting vs)
    {
        IList list = (IList)ListInput.GetValue(vs);
        return list;
    }
}