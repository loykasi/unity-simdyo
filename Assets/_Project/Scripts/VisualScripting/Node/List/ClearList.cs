using System;
using System.Collections;
using UnityEngine;

namespace Loykas.Scripting
{


    class ClearListNode : ScriptNode
    {
        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue ListInput;

        public ClearListNode()
        {
            Enter = InputTrigger(nameof(Enter), Clear);
            Exit = OutputTrigger(nameof(Exit));

            ListInput = InputValue(nameof(ListInput), ScriptDataType.List(DataType.Any));
        }

        private OutputTrigger Clear(ScriptFlow vs)
        {
            IList list = (IList)ListInput.GetValue(vs);
            list.Clear();
            return Exit;
        }
    }
}