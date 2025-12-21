using System;
using System.Collections;
using UnityEngine;

namespace Loykas.Scripting
{
    class SetListItemNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.List;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue ListInput;
        public InputValue Value;
        public InputValue Index;

        public override ScriptNode Create()
        {
            return new SetListItemNode();
        }

        public SetListItemNode()
        {
            Enter = CreateInputTrigger(nameof(Enter), Set);
            Exit = OutputTrigger(nameof(Exit));

            ListInput = InputValue(nameof(ListInput), ScriptDataType.List(DataType.Any))
                        .UseGlobalLocalized();
            
            Value = InputValue(nameof(Value), ScriptDataType.Single(DataType.Any))
                    .UseGlobalLocalized();
            
            Index = InputValue(nameof(Index), ScriptDataType.Single(DataType.Number))
                    .UseInput()
                    .UseGlobalLocalized();

            ListInput.OnConnected += OnListInputConnected;
        }

        private OutputTrigger Set()
        {
            IList list = (IList)ListInput.GetValue();
            object item = Value.GetValue();
            int index = (int)(float)Index.GetValue();

            if (index >= 0 && index < list.Count)
            {
                list[index] = item;    
            }
            
            return Exit;
        }

        private void OnListInputConnected()
        {
            UpdateNode();
        }

        public override void UpdateNode()
        {
            if (ListInput.Source != null)
            {
                DataType type = ListInput.Source.Type.Type;
                
                Value.SetType(ScriptDataType.Single(type));
            }
            else
            {
                Value.SetType(ScriptDataType.Single(DataType.Any));
            }

            OnNodeUpdated?.Invoke();
        }
    }
}