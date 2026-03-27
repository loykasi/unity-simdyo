using System;
using System.Collections.Generic;

namespace Loykas.Scripting
{
    public class InputTrigger : Port<OutputTrigger>
    {
        public Func<NodeTask, OutputTrigger> Action;
        public List<OutputTrigger> Sources = new();
        public OutputTrigger TargetOutputTrigger;

        public void Init
        (
            IScriptNode node,
            string key,
            Func<NodeTask,OutputTrigger> action
        )
        {
            Init(node, key);
            Action = action;
        }

        public override bool CanConnectTo(OutputTrigger port)
        {
            return true;
        }

        public override void Connect(OutputTrigger port)
        {
            Sources.Add(port);
        }

        public bool Invoke(NodeTask task)
        {
            TargetOutputTrigger = Action?.Invoke(task);
            return !(Node.HasOutputTriggers && TargetOutputTrigger == null);
        }

        protected override void DisconnectPort(OutputTrigger port)
        {
            if (!Sources.Contains(port))
            {
                Sources.Remove(port);
            }
        }

        public override void Release()
        {
            base.Release();
            Sources.Clear();
            Action = null;
            TargetOutputTrigger = null;

            ScriptPool.Instance.InputTrigger.Release(this);
        }
    }
}