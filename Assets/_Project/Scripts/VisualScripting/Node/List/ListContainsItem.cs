using System;
using System.Collections;
using UnityEngine;

namespace Loykas.Scripting
{
    class ListContainsItemNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.List;

        public InputValue ListInput;
        public InputValue Item;
        public OutputValue Output;

        public override ScriptNode Create()
        {
            return new ListContainsItemNode();
        }

        public ListContainsItemNode()
        {
            ListInput = InputValue(nameof(ListInput), ScriptDataType.List(DataType.Any));
            Item = InputValue(nameof(Item), ScriptDataType.Single(DataType.Any));
            Output = OutputValue(nameof(Output), ScriptDataType.Single(DataType.Boolean), Get);

            ListInput.OnConnected += OnListInputConnected;
        }

        private object Get()
        {
            IList list = (IList)ListInput.GetValue();
            object item = Item.GetValue();
            return list.Contains(item);
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