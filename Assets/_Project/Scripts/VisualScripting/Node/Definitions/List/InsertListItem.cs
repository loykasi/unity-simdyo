using System.Collections;

namespace Loykas.Scripting
{
    class InsertListItemNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.List;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue ListInput;
        public InputValue Value;
        public InputValue Index;

        public override ScriptNode Create()
        {
            return new InsertListItemNode();
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
                ScriptDataType.Any(),
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
            int index = (int)Index.GetValue().NumberValue;
            list.Insert(index, item);
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