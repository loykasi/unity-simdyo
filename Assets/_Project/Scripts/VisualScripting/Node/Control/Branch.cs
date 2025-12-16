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
            IfTrue = OutputTrigger(nameof(IfTrue));
            IfFalse = OutputTrigger(nameof(IfFalse));
            
            Condition = InputValue(nameof(Condition), ScriptDataType.Single(DataType.Boolean));
        }

        public override ScriptNode Create()
        {
            return new BranchNode();
        }

        private OutputTrigger Branching()
        {
            if ((bool)Condition.GetValue())
            {
                return IfTrue;
            }
            return IfFalse;
        }
    }
}