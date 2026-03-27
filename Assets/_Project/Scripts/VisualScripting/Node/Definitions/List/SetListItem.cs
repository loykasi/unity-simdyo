using System.Collections;

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

        public override void Build()
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
            
            Index = CreateInputValue
            (
                nameof(Index),
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    LocalizationKey = nameof(Index)
                }
            ).UseInput();

            ListInput.OnConnected += OnListInputConnected;
        }

        private OutputTrigger Set(NodeTask task)
        {
            IList list = ListInput.GetValue().ListValue;
            object item = Value.GetValue();
            
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