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
    public ValueInput ListInput;
    public ValueInput IndexInput;
    public ValueOutput Output;

    public GetListItemNode(string title) : base(title)
    {
        ListInput = ValueInput(DataType.List, false);
        IndexInput = ValueInput(DataType.Number, true);
        Output = ValueOutput(DataType.List, Get);
    }

    private object Get(VisualScripting vs)
    {
        IList list = (IList)ListInput.GetValue(vs);
        int index = (int)(float)IndexInput.GetValue(vs);
        return list[index];
    }
}