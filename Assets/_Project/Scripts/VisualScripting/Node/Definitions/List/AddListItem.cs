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
            Exit = CreateOutputTrigger(nameof(Exit));

            ListInput = CreateInputValue
            (
                nameof(ListInput),
                ScriptDataType.List(DataType.Any),
                new PortSettings
                {
                    LocalizationKey = nameof(ListInput)
                }
            );
            
            Value = CreateInputValue
            (
                nameof(Value),
                ScriptDataType.Single(DataType.Any),
                new PortSettings
                {
                    LocalizationKey = nameof(Value)
                }
            );

            ListInput.OnConnected += OnListInputConnected;
            ListInput.OnDisconnected += OnListInputDisconnected;
        }

        private OutputTrigger Set(NodeTask task)
        {
            IList list = ListInput.GetValue().ListValue;
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