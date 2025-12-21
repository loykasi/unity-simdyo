using System.Collections;
using UnityEngine;

namespace Loykas.Scripting
{
    class GetListLengthNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.List;

        public InputValue ListInput;
        public OutputValue Value;

        public override ScriptNode Create()
        {
            return new GetListLengthNode();
        }

        public GetListLengthNode()
        {
            ListInput = InputValue(nameof(ListInput), ScriptDataType.List(DataType.Any))
                        .UseGlobalLocalized();

            Value = OutputValue(nameof(Value), ScriptDataType.Single(DataType.Number), Get)
                    .HideLabel();
        }

        private object Get()
        {
            IList list = (IList)ListInput.GetValue();
            return list.Count;
        }
    }
}