using UnityEngine;

namespace Loykas.Scripting
{
    class ResumeNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Control;

        public InputTrigger Enter;

        public ResumeNode()
        {
            Enter = CreateInputTrigger(nameof(Enter), Resume);
        }

        public override ScriptNode Create()
        {
            return new ResumeNode();
        }

        private OutputTrigger Resume()
        {
            SceneManager.Instance.Resume();
            
            return null;
        }
    }
}