using System;
using System.Collections;
using UnityEngine;

namespace Loykas.Scripting
{
    class ClearListNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.List;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue ListInput;

        public override ScriptNode Create()
        {
            return new ClearListNode();
        }

        public ClearListNode()
        {
            Enter = CreateInputTrigger(nameof(Enter), Clear);
            Exit = CreateOutputTrigger(nameof(Exit));

            ListInput = CreateInputValue
            (
                nameof(ListInput),
                ScriptDataType.List(DataType.Any),
                new PortSettings
                {
                    LocalizationKey = nameof(ListInput)
                }
            );
        }

        private OutputTrigger Clear(NodeTask task)
        {
            IList list = ListInput.GetValue().ListValue;
            list.Clear();
            return Exit;
        }
    }
}