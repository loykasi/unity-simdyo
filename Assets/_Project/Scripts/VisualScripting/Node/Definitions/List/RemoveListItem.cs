using System.Collections;

namespace Loykas.Scripting
{
    class RemoveListItemNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.List;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue ListInput;
        public InputValue Index;

        public override ScriptNode Create()
        {
            return new RemoveListItemNode();
        }

        public override void Build()
        {
            Enter = CreateInputTrigger(nameof(Enter), Set);
            
            Exit = CreateOutputTrigger
            (
                nameof(Exit),
                new PortSettings
                {
                    HideLabel = true
                }
            );

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
        }

        private OutputTrigger Set(NodeTask task)
        {
            IList list = ListInput.GetValue().ListValue;
            int index = (int)Index.GetValue().NumberValue;
            
            if (index >= 0 && index < list.Count)
            {
                list.RemoveAt(index);   
            }
            
            return Exit;
        }
    }
}