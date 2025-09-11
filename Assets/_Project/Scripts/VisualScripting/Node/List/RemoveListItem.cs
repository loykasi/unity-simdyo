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

    public InputValue ListInput;
    public InputValue Index;
    public OutputValue Output;

    public RemoveListItemNode(string title) : base(title)
    {
        Enter = InputTrigger(nameof(Enter), Set);
        Exit = OutputTrigger(nameof(Exit));

        ListInput = InputValue(nameof(ListInput), DataType.List);
        Index = InputValue(nameof(Index));
        Output = OutputValue(nameof(Output), DataType.List, Get);
    }

    private OutputTrigger Set(ScriptFlow vs)
    {
        IList list = (IList)ListInput.GetValue(vs);
        int index = (int)(float)Index.GetValue(vs);
        list.RemoveAt(index);
        return Exit;
    }

    private object Get(ScriptFlow vs)
    {
        IList list = (IList)ListInput.GetValue(vs);
        return list;
    }
}