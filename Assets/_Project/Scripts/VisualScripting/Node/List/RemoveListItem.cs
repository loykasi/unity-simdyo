using System;
using System.Collections;
using UnityEngine;

namespace Loykas.Scripting
{

    class RemoveListItemNode : ScriptNode
    {
        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue ListInput;
        public InputValue Index;
        public OutputValue Output;

        public RemoveListItemNode()
        {
            Enter = InputTrigger(nameof(Enter), Set);
            Exit = OutputTrigger(nameof(Exit));

            ListInput = InputValue(nameof(ListInput), ScriptDataType.List(DataType.Any));
            Index = InputValue(nameof(Index));
            Output = OutputValue(nameof(Output), ScriptDataType.List(DataType.Any), Get);
        }

        private OutputTrigger Set(ScriptFlow vs)
        {
            IList list = (IList)ListInput.GetValue(vs);
            int index = (int)(float)Index.GetValue(vs);
            list.RemoveAt(index);
            return Exit;
        }

        private object Get(ScriptFlow vs)
        {
            IList list = (IList)ListInput.GetValue(vs);
            return list;
        }
    }
}