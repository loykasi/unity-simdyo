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
        ListInput = InputValue(nameof(ListInput), ScriptDataType.List(DataType.Any));
        Index = InputValue(nameof(Index), ScriptDataType.Single(DataType.Number)).UseInput();
        Output = OutputValue(nameof(Output), ScriptDataType.List(DataType.Any), Get);
    }

    private object Get(ScriptFlow vs)
    {
        IList list = (IList)ListInput.GetValue(vs);
        int index = (int)(float)Index.GetValue(vs);
        return list[index];
    }
}