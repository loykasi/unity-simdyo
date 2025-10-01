using System;
using System.Collections.Generic;

namespace Loykas.Scripting
{
    public class NodeTask
    {
        public OutputTrigger From;
        public InputTrigger Trigger;

        public Stack<InputTrigger> _loops = new();

        public void Invoke(ScriptFlow flow)
        {
            while (Trigger != null)
            {
                bool isDone = Trigger.Invoke(flow);

                if (!isDone)
                {
                    return;
                }

                Trigger = Trigger.TargetOutputTrigger.Invoke(flow);
                if (Trigger == null)
                {
                    if (IsInLoop())
                    {
                        ExitLoop();
                        continue;
                    }
                    else
                    {
                        flow.RemoveTask(this);
                        return;
                    }
                }
            }
        }

        public void EnterLoop(InputTrigger trigger)
        {
            _loops.Push(trigger);
        }

        public void ExitLoop()
        {
            Trigger = _loops.Pop();
        }

        private bool IsInLoop()
        {
            return _loops.Count > 0;
        }
    }
}