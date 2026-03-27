using System.Collections;

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

        public override void Build()
        {
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
            
            Output = CreateOutputValue
            (
                nameof(Output),
                Get,
                ScriptDataType.Single(DataType.Boolean),
                new PortSettings
                {
                    HideLabel = true
                }
            );

            ListInput.OnConnected += OnListInputConnected;
        }

        private ValueTransfer Get()
        {
            IList list = ListInput.GetValue().ListValue;
            object item = Value.GetValue();
            return ValueTransfer.CreateBool(list.Contains(item));
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