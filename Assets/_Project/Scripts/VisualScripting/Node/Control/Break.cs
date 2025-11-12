using UnityEngine;

namespace Loykas.Scripting
{
    class BreakNode : ScriptNode
    {
        public InputTrigger Enter;

        public BreakNode()
        {
            Enter = InputTrigger(nameof(Enter), BreakLoop);
        }

        private OutputTrigger BreakLoop()
        {
            Flow.BreakLoop();

            return null;
        }
    }
}