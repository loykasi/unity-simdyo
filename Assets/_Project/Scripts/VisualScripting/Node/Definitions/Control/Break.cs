using UnityEngine;

namespace Loykas.Scripting
{
    class BreakNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Control;

        public InputTrigger Enter;

        public override void Build()
        {
            Enter = CreateInputTrigger(nameof(Enter), BreakLoop);
        }

        public override ScriptNode Create()
        {
            return new BreakNode();
        }

        private OutputTrigger BreakLoop(NodeTask task)
        {
            task.BreakLoop();
            
            return null;
        }
    }
}