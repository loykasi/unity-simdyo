using System;
using System.Collections;
using UnityEngine;

namespace Loykas.Scripting
{

    class ListContainsItemNode : ScriptNode
    {
        public InputValue ListInput;
        public InputValue Item;
        public OutputValue Output;

        public ListContainsItemNode()
        {
            ListInput = InputValue(nameof(ListInput), ScriptDataType.List(DataType.Any));
            Item = InputValue(nameof(Item));
            Output = OutputValue(nameof(Output), ScriptDataType.Single(DataType.Boolean), Get);
        }

        private object Get(ScriptFlow vs)
        {
            IList list = (IList)ListInput.GetValue(vs);
            object item = Item.GetValue(vs);
            return list.Contains(item);
        }
    }
}