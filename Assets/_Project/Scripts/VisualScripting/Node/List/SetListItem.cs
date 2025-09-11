using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "SetItem", menuName = "Scriptable Objects/Visual Scripting/Node/List/Set Item")]
public class SetListItem : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new SetListItemNode(Title);
    }
}

class SetListItemNode : ScriptNode
{
    public InputTrigger Enter;
    public OutputTrigger Exit;

    public InputValue ListInput;
    public InputValue Item;
    public InputValue Index;
    public OutputValue Output;

    public SetListItemNode(string title) : base(title)
    {
        Enter = InputTrigger(nameof(Enter), Set);
        Exit = OutputTrigger(nameof(Exit));

        ListInput = InputValue(nameof(ListInput), DataType.List);
        Item = InputValue(nameof(Item));
        Index = InputValue(nameof(Index));
        Output = OutputValue(nameof(Output), DataType.List, Get);
    }

    private OutputTrigger Set(ScriptFlow vs)
    {
        IList list = (IList)ListInput.GetValue(vs);
        object item = Item.GetValue(vs);
        int index = (int)(float)Index.GetValue(vs);
        list[index] = item;
        return Exit;
    }

    private object Get(ScriptFlow vs)
    {
        IList list = (IList)ListInput.GetValue(vs);
        return list;
    }
}