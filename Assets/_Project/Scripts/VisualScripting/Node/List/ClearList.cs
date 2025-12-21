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
            Exit = OutputTrigger(nameof(Exit));

            ListInput = InputValue(nameof(ListInput), ScriptDataType.List(DataType.Any))
                        .UseGlobalLocalized();
        }

        private OutputTrigger Clear()
        {
            IList list = (IList)ListInput.GetValue();
            list.Clear();
            return Exit;
        }
    }
}