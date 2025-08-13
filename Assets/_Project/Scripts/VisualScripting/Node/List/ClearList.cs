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

    public InputValue ListInput;

    public ClearListNode(string title) : base(title)
    {
        Enter = InputTrigger(nameof(Enter), Clear);
        Exit = OutputTrigger(nameof(Exit));

        ListInput = InputValue(nameof(ListInput), DataType.List, false);
    }

    private OutputTrigger Clear(VisualScripting vs)
    {
        IList list = (IList)ListInput.GetValue(vs);
        list.Clear();
        return Exit;
    }
}