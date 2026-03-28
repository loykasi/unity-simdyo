using UnityEngine;

namespace Loykas.Scripting
{
    class BranchNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Control;

        public InputTrigger Enter;
        public OutputTrigger IfTrue;
        public OutputTrigger IfFalse;

        public InputValue Condition;

        public override void Build()
        {
            Enter = CreateInputTrigger(nameof(Enter), Branching);

            IfTrue = CreateOutputTrigger(nameof(IfTrue), PortSettings.Default);
            IfFalse = CreateOutputTrigger(nameof(IfFalse), PortSettings.Default);
            
            Condition = CreateInputValue
            (
                nameof(Condition),
                ScriptDataType.Single(DataType.Boolean),
                new PortSettings
                {
                    IsLocalizationDisabled = true,
                }
            );
        }

        public override ScriptNode Create()
        {
            return new BranchNode();
        }

        private OutputTrigger Branching(NodeTask task)
        {
            if (Condition.GetValue().BoolValue)
            {
                return IfTrue;
            }
            return IfFalse;
        }
    }
}