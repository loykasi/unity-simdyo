using System.Collections;
using UnityEngine;

namespace Loykas.Scripting
{
    class GetListLengthNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.List;

        public InputValue ListInput;
        public OutputValue Output;

        public override ScriptNode Create()
        {
            return new GetListLengthNode();
        }

        public GetListLengthNode()
        {
            ListInput = InputValue(nameof(ListInput), ScriptDataType.List(DataType.Any));
            Output = OutputValue(nameof(Output), ScriptDataType.Single(DataType.Number), Get);
        }

        private object Get()
        {
            IList list = (IList)ListInput.GetValue();
            return list.Count;
        }
    }
}