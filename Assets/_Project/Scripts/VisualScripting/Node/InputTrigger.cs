using System;
using System.Collections.Generic;

namespace Loykas.Scripting
{
    public class InputTrigger : Port<OutputTrigger>
    {
        public Func<OutputTrigger> Action;
        public List<OutputTrigger> Sources = new();

        public OutputTrigger TargetOutputTrigger;
        public bool IsDone = true;

        public InputTrigger(string key, Func<OutputTrigger> action) : base(key)
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
            OutputTrigger trigger = Action?.Invoke();
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

        public InputTrigger NoLocalize()
        {
            ShouldLocalized = false;
            return this;
        }
    }
}