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

        public BranchNode()
        {
            Enter = CreateInputTrigger(nameof(Enter), Branching);

            PortSettings outputPortSettings = PortSettings.Default;
            outputPortSettings.HideLabel = true;

            IfTrue = CreateOutputTrigger(nameof(IfTrue), outputPortSettings);
            IfFalse = CreateOutputTrigger(nameof(IfFalse), outputPortSettings);
            
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