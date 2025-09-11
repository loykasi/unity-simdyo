using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "GetListItem", menuName = "Scriptable Objects/Visual Scripting/Node/List/Get Item")]
public class GetListItem : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new GetListItemNode(Title);
    }
}

class GetListItemNode : ScriptNode
{
    public InputValue ListInput;
    public InputValue Index;
    public OutputValue Output;

    public GetListItemNode(string title) : base(title)
    {
        ListInput = InputValue(nameof(ListInput), DataType.List);
        Index = InputValue(nameof(Index), DataType.Number);
        Output = OutputValue(nameof(Output), DataType.List, Get);
    }

    private object Get(ScriptFlow vs)
    {
        IList list = (IList)ListInput.GetValue(vs);
        int index = (int)(float)Index.GetValue(vs);
        return list[index];
    }
}