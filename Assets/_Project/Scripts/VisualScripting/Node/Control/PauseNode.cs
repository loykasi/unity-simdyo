using UnityEngine;

namespace Loykas.Scripting
{
    class PauseNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Control;

        public InputTrigger Enter;

        public PauseNode()
        {
            Enter = CreateInputTrigger(nameof(Enter), Pause);
        }

        public override ScriptNode Create()
        {
            return new PauseNode();
        }

        private OutputTrigger Pause()
        {
            SceneManager.Instance.Pause();
            
            return null;
        }
    }
}