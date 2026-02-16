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
        public InputValue Value;

        public override ScriptNode Create()
        {
            return new AddListItemNode();
        }

        public AddListItemNode()
        {
            Enter = CreateInputTrigger(nameof(Enter), Set);
            Exit = OutputTrigger(nameof(Exit));

            ListInput = InputValue(nameof(ListInput), ScriptDataType.List(DataType.Any))
                        .UseGlobalLocalized();
            
            Value = InputValue(nameof(Value), ScriptDataType.Single(DataType.Any))
                    .UseGlobalLocalized();

            ListInput.OnConnected += OnListInputConnected;
            ListInput.OnDisconnected += OnListInputDisconnected;
        }

        private OutputTrigger Set(NodeTask task)
        {
            IList list = (IList)ListInput.GetValue();
            object item = Value.GetValue();
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