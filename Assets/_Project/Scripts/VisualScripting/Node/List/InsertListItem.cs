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

    public InputValue ListInput;
    public InputValue Item;
    public InputValue Index;
    public OutputValue Output;

    public InsertListItemNode(string title) : base(title)
    {
        Enter = InputTrigger(nameof(Enter), Set);
        Exit = OutputTrigger(nameof(Exit));

        ListInput = InputValue(nameof(ListInput), DataType.List, false);
        Item = InputValue(nameof(Item), true);
        Index = InputValue(nameof(Index), true);
        Output = OutputValue(nameof(Output), DataType.List, Get);
    }

    private OutputTrigger Set(VisualScripting vs)
    {
        IList list = (IList)ListInput.GetValue(vs);
        object item = Item.GetValue(vs);
        int index = (int)(float)Index.GetValue(vs);
        list.Insert(index, item);
        return Exit;
    }

    private object Get(VisualScripting vs)
    {
        IList list = (IList)ListInput.GetValue(vs);
        return list;
    }
}