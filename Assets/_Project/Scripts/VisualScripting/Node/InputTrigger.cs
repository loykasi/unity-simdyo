using System;
using System.Collections.Generic;

namespace Loykas.Scripting
{
    public class InputTrigger : Port<OutputTrigger>
    {
        public Func<ScriptFlow, OutputTrigger> Action;
        public List<OutputTrigger> Sources = new();

        public OutputTrigger TargetOutputTrigger;
        public bool IsDone = true;

        public InputTrigger(string key, Func<ScriptFlow, OutputTrigger> action) : base(key)
        {
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

        public bool Invoke(ScriptFlow vs)
        {
            OutputTrigger trigger = Action?.Invoke(vs);
            TargetOutputTrigger = trigger;
            return trigger != null;
        }

        protected override void DisconnectPort(OutputTrigger port)
        {
            if (!Sources.Contains(port))
            {
                Sources.Remove(port);
            }
        }
    }
}