using UnityEngine;

namespace Loykas.Scripting
{
    class BreakNode : ScriptNode
    {
        public InputTrigger Enter;

        public BreakNode()
        {
            Enter = InputTrigger(
                nameof(Enter),
                (vs) =>
                {
                    vs.BreakLoop();

                    return null;
                }
            );
        }
    }
}