using System;
using System.Collections;
using UnityEngine;

namespace Loykas.Scripting
{
    class RemoveListItemNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.List;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue ListInput;
        public InputValue Index;
        public OutputValue Output;

        public override ScriptNode Create()
        {
            return new RemoveListItemNode();
        }

        public RemoveListItemNode()
        {
            Enter = CreateInputTrigger(nameof(Enter), Set);
            Exit = OutputTrigger(nameof(Exit));

            ListInput = InputValue(nameof(ListInput), ScriptDataType.List(DataType.Any));
            Index = InputValue(nameof(Index));
            Output = OutputValue(nameof(Output), ScriptDataType.List(DataType.Any), Get);
        }

        private OutputTrigger Set()
        {
            IList list = (IList)ListInput.GetValue();
            int index = (int)(float)Index.GetValue();
            list.RemoveAt(index);
            return Exit;
        }

        private object Get()
        {
            IList list = (IList)ListInput.GetValue();
            return list;
        }
    }
}