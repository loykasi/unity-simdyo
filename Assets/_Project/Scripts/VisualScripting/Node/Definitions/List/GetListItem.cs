using System.Collections;

namespace Loykas.Scripting
{
    class GetListItemNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.List;

        public InputValue ListInput;
        public InputValue Index;
        public OutputValue Value;

        public override ScriptNode Create()
        {
            return new GetListItemNode();
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
            
            Index = CreateInputValue
            (
                nameof(Index),
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    LocalizationKey = nameof(Index)
                }
            ).UseInput();

            Value = CreateOutputValue
            (
                nameof(Value),
                Get,
                ScriptDataType.Single(DataType.Any),
                new PortSettings
                {
                    LocalizationKey = nameof(Value)
                }
            );

            ListInput.OnConnected += OnListInputConnected;
        }

        public override void Init()
        {
            UpdateNode();
        }

        private ValueTransfer Get()
        {
            IList list = ListInput.GetValue().ListValue;

            object value = Index.GetValue();
            int index;
            if (value is float floatValue)
            {
                index = (int)floatValue;
            }
            else
            {
                index = (int)value;
            }
            
            return ValueTransfer.FromValue(list[index]);
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