using System.Collections;
using UnityEngine;

namespace Loykas.Scripting
{
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
        public InputValue ListInput;
        public OutputValue Output;

        public GetListLengthNode(string title) : base(title)
        {
            ListInput = InputValue(nameof(ListInput), ScriptDataType.List(DataType.Any));
            Output = OutputValue(nameof(Output), ScriptDataType.List(DataType.Any), Get);
        }

        private object Get(ScriptFlow vs)
        {
            IList list = (IList)ListInput.GetValue(vs);
            return list.Count;
        }
    }
}