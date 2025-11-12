using System.Collections;
using UnityEngine;

namespace Loykas.Scripting
{

    class GetListLengthNode : ScriptNode
    {
        public InputValue ListInput;
        public OutputValue Output;

        public GetListLengthNode()
        {
            ListInput = InputValue(nameof(ListInput), ScriptDataType.List(DataType.Any));
            Output = OutputValue(nameof(Output), ScriptDataType.List(DataType.Any), Get);
        }

        private object Get()
        {
            IList list = (IList)ListInput.GetValue();
            return list.Count;
        }
    }
}