using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "GetListLength", menuName = "Scriptable Objects/Visual Scripting/Node/List/Get Length")]
public class GetListLength : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new GetListLengthNode(Title);
    }
}

class GetListLengthNode : ScriptNode
{
    public ValueInput ListInput;
    public ValueOutput Output;

    public GetListLengthNode(string title) : base(title)
    {
        ListInput = ValueInput(DataType.List, false);
        Output = ValueOutput(DataType.List, Get);
    }

    private object Get(VisualScripting vs)
    {
        IList list = (IList)ListInput.GetValue(vs);
        return list.Count;
    }
}