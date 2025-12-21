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

        public override ScriptNode Create()
        {
            return new RemoveListItemNode();
        }

        public RemoveListItemNode()
        {
            Enter = CreateInputTrigger(nameof(Enter), Set);
            Exit = OutputTrigger(nameof(Exit));

            ListInput = InputValue(nameof(ListInput), ScriptDataType.List(DataType.Any))
                        .UseGlobalLocalized();

            Index = InputValue(nameof(Index), ScriptDataType.Single(DataType.Number))
                    .UseInput()
                    .UseGlobalLocalized();
        }

        private OutputTrigger Set()
        {
            IList list = (IList)ListInput.GetValue();
            int index = (int)(float)Index.GetValue();
            
            if (index >= 0 && index < list.Count)
            {
                list.RemoveAt(index);   
            }
            
            return Exit;
        }
    }
}