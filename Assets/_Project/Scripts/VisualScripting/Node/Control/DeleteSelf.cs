using UnityEngine;

namespace Loykas.Scripting
{
    class DeleteSelfNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Control;
        public override bool CanUseGlobal => false;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public override ScriptNode Create()
        {
            return new DeleteSelfNode();
        }

        public DeleteSelfNode()
        {
            Enter = CreateInputTrigger(nameof(Enter), Delete);
            Exit = OutputTrigger(nameof(Exit));
        }

        private OutputTrigger Delete(NodeTask task)
        {
            ObjectManager.Instance.DeleteEntity(Flow.Entity);
            return Exit;
        }
    }
}