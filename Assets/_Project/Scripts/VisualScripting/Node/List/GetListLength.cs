using System.Collections;
using UnityEngine;

namespace Loykas.Scripting
{

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