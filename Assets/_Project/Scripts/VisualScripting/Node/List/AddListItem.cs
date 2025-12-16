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

        public override ScriptNode Create()
        {
            return new AddListItemNode();
        }

        public AddListItemNode()
        {
            Enter = CreateInputTrigger(nameof(Enter), Set);
            Exit = OutputTrigger(nameof(Exit)).HideLabel();

            ListInput = InputValue(nameof(ListInput), ScriptDataType.List(DataType.Any)).HideLabel();
            Item = InputValue(nameof(Item), ScriptDataType.Single(DataType.Any)).HideLabel();

            ListInput.OnConnected += OnListInputConnected;
            ListInput.OnDisconnected += OnListInputDisconnected;
        }

        private OutputTrigger Set()
        {
            IList list = (IList)ListInput.GetValue();
            object item = Item.GetValue();
            list.Add(item);
            return Exit;
        }

        private void OnListInputConnected()
        {
            UpdateNode();
        }

        private void OnListInputDisconnected()
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