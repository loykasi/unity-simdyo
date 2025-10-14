using UnityEngine;

namespace Loykas.Scripting
{

    class BranchNode : ScriptNode
    {
        public InputTrigger Enter;
        public OutputTrigger IfTrue;
        public OutputTrigger IfFalse;

        public InputValue Condition;

        public BranchNode(string title) : base(title)
        {
            Enter = InputTrigger(
                nameof(Enter),
                (vs) =>
                {
                    if ((bool)Condition.GetValue(vs))
                    {
                        return IfTrue;
                    }
                    return IfFalse;
                }
            );
            IfTrue = OutputTrigger(nameof(IfTrue));
            IfFalse = OutputTrigger(nameof(IfFalse));
            Condition = InputValue(nameof(Condition));
        }
    }
}