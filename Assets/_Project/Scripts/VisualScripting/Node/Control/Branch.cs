using UnityEngine;

namespace Loykas.Scripting
{

    class BranchNode : ScriptNode
    {
        public InputTrigger Enter;
        public OutputTrigger IfTrue;
        public OutputTrigger IfFalse;

        public InputValue Condition;

        public BranchNode()
        {
            Enter = InputTrigger(nameof(Enter), Branching);
            IfTrue = OutputTrigger(nameof(IfTrue));
            IfFalse = OutputTrigger(nameof(IfFalse));
            Condition = InputValue(nameof(Condition));
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