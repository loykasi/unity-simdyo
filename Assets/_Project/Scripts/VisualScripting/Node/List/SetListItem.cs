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
        public InputValue Item;
        public InputValue Index;

        public override ScriptNode Create()
        {
            return new SetListItemNode();
        }

        public SetListItemNode()
        {
            Enter = CreateInputTrigger(nameof(Enter), Set);
            Exit = OutputTrigger(nameof(Exit));

            ListInput = InputValue(nameof(ListInput), ScriptDataType.List(DataType.Any));
            Item = InputValue(nameof(Item), ScriptDataType.Single(DataType.Any));
            Index = InputValue(nameof(Index), ScriptDataType.Single(DataType.Number)).UseInput();

            ListInput.OnConnected += OnListInputConnected;
        }

        private OutputTrigger Set()
        {
            IList list = (IList)ListInput.GetValue();
            object item = Item.GetValue();
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
                
                Item.SetType(ScriptDataType.Single(type));
            }
            else
            {
                Item.SetType(ScriptDataType.Single(DataType.Any));
            }

            OnNodeUpdated?.Invoke();
        }
    }
}