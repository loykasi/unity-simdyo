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
            ListInput = CreateInputValue
            (
                nameof(ListInput),
                ScriptDataType.List(DataType.Any),
                new PortSettings
                {
                    LocalizationKey = nameof(ListInput)
                }
            );

            Value = CreateOutputValue
            (
                nameof(Value),
                Get,
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    HideLabel = true
                }
            );
        }

        private ValueTransfer Get()
        {
            IList list = ListInput.GetValue().ListValue;
            return ValueTransfer.CreateNumber(list.Count);
        }
    }
}