using System;
using System.Collections;
using UnityEngine;

namespace Loykas.Scripting
{

    class AddListItemNode : ScriptNode
    {
        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue ListInput;
        public InputValue Item;
        public OutputValue Output;

        public AddListItemNode()
        {
            Enter = InputTrigger(nameof(Enter), Set);
            Exit = OutputTrigger(nameof(Exit));

            ListInput = InputValue(nameof(ListInput), ScriptDataType.List(DataType.Any));
            Item = InputValue(nameof(Item));
            Output = OutputValue(nameof(Output), ScriptDataType.List(DataType.Any), Get);
        }

        private OutputTrigger Set(ScriptFlow vs)
        {
            IList list = (IList)ListInput.GetValue(vs);
            object item = Item.GetValue(vs);
            list.Add(item);
            return Exit;
        }

        private object Get(ScriptFlow vs)
        {
            IList list = (IList)ListInput.GetValue(vs);
            return list;
        }
    }
}