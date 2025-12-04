using System;
using System.Collections;
using UnityEngine;

namespace Loykas.Scripting
{
    class AddListItemNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.List;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue ListInput;
        public InputValue Item;
        public OutputValue Output;

        public override ScriptNode Create()
        {
            return new AddListItemNode();
        }

        public AddListItemNode()
        {
            Enter = CreateInputTrigger(nameof(Enter), Set);
            Exit = OutputTrigger(nameof(Exit));

            ListInput = InputValue(nameof(ListInput), ScriptDataType.List(DataType.Any));
            Item = InputValue(nameof(Item));
            Output = OutputValue(nameof(Output), ScriptDataType.List(DataType.Any), Get);
        }

        private OutputTrigger Set()
        {
            IList list = (IList)ListInput.GetValue();
            object item = Item.GetValue();
            list.Add(item);
            return Exit;
        }

        private object Get()
        {
            IList list = (IList)ListInput.GetValue();
            return list;
        }
    }
}