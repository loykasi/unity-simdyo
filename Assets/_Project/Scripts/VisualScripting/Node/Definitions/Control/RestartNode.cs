using UnityEngine;

namespace Loykas.Scripting
{
    class RestartNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Control;

        public InputTrigger Enter;

        public override void Build()
        {
            Enter = CreateInputTrigger(nameof(Enter), Restart);
        }

        public override ScriptNode Create()
        {
            return new RestartNode();
        }

        private OutputTrigger Restart(NodeTask task)
        {
            SceneManager.Instance.Restart();
            
            return null;
        }
    }
}