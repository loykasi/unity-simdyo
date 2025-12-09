using System.Collections;
using UnityEngine;

namespace Loykas.Scripting
{
    class InsertListItemNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.List;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue ListInput;
        public InputValue Item;
        public InputValue Index;
        public OutputValue Output;

        public override ScriptNode Create()
        {
            return new InsertListItemNode();
        }

        public InsertListItemNode()
        {
            Enter = CreateInputTrigger(nameof(Enter), Set);
            Exit = OutputTrigger(nameof(Exit));

            ListInput = InputValue(nameof(ListInput), ScriptDataType.List(DataType.Any));
            Item = InputValue(nameof(Item));
            Index = InputValue(nameof(Index), ScriptDataType.Single(DataType.Number)).UseInput();
            Output = OutputValue(nameof(Output), ScriptDataType.List(DataType.Any), Get);
        }

        private OutputTrigger Set()
        {
            IList list = (IList)ListInput.GetValue();
            object item = Item.GetValue();
            int index = (int)(float)Index.GetValue();
            list.Insert(index, item);
            return Exit;
        }

        private object Get()
        {
            IList list = (IList)ListInput.GetValue();
            return list;
        }
    }
}