using System;
using System.Collections;
using UnityEngine;

namespace Loykas.Scripting
{
    class ListContainsItemNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.List;

        public InputValue ListInput;
        public InputValue Value;
        public OutputValue Output;

        public override ScriptNode Create()
        {
            return new ListContainsItemNode();
        }

        public ListContainsItemNode()
        {
            ListInput = InputValue(nameof(ListInput), ScriptDataType.List(DataType.Any))
                        .UseGlobalLocalized();
                        
            Value = InputValue(nameof(Value), ScriptDataType.Single(DataType.Any))
                    .UseGlobalLocalized();
            
            Output = OutputValue(nameof(Output), ScriptDataType.Single(DataType.Boolean), Get)
                    .HideLabel();

            ListInput.OnConnected += OnListInputConnected;
        }

        private object Get()
        {
            IList list = (IList)ListInput.GetValue();
            object item = Value.GetValue();
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