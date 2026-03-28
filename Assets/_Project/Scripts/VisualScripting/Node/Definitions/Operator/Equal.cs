namespace Loykas.Scripting
{
    class EqualNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Operator;

        public InputValue A;
        public InputValue B;
        public OutputValue Output;

        public override ScriptNode Create()
        {
            return new EqualNode();
        }

        public override void Build()
        {
            A = CreateInputValue
            (
                nameof(A),
                ScriptDataType.Single(DataType.Any),
                new PortSettings
                {
                    IsLocalizationDisabled = true
                }
            )
            .UseInput();
            
            B = CreateInputValue
            (
                nameof(B),
                ScriptDataType.Single(DataType.Any),
                new PortSettings
                {
                    IsLocalizationDisabled = true
                }
            )
            .UseInput();

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

            A.OnConnected += OnAConnected;
            B.OnConnected += OnBConnected;
        }

        private ValueTransfer Get()
        {
            ValueTransfer a = A.GetValue();
            ValueTransfer b = B.GetValue();

            return ValueTransfer.CreateBool(a.GetObjectValue() == b.GetObjectValue());
        }

        private void OnAConnected()
        {
            // if (A.Source != null)
            // {
            //     ScriptDataType type = A.Source.Type;
            //     B.SetType(type);
            // }
            // else
            // {
            //     A.SetType(ScriptDataType.Single(DataType.Any));
            // }

            // OnNodeUpdated?.Invoke();
        }

        private void OnBConnected()
        {
            
        }
    }
}