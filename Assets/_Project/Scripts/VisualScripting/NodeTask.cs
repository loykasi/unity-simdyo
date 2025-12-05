using System;
using System.Collections.Generic;
using UnityEngine;

namespace Loykas.Scripting
{
    public class NodeTask
    {
        public bool IsDone;
        public OutputTrigger From;
        public InputTrigger Trigger;

        public Stack<InputTrigger> _loops = new();
        public bool ShouldBreak;

        private bool _shouldRemoveOnDone;

        public NodeTask SetRemoveOnDone()
        {
            _shouldRemoveOnDone = true;
            return this;
        }

        public void Invoke(ScriptFlow flow)
        {
            IsDone = false;
            while (Trigger != null)
            {
                bool isDone = Trigger.Invoke(flow);

                if (!isDone)
                {
                    return;
                }

                Trigger = Trigger.TargetOutputTrigger?.Destination;
                if (Trigger == null)
                {
                    if (IsInLoop())
                    {
                        ExitLoop();
                        continue;
                    }
                    else
                    {
                        if (_shouldRemoveOnDone)
                        {
                            flow.RemoveTask(this);
                        }

                        IsDone = true;
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

        public void BreakLoop()
        {
            ShouldBreak = true;
        }

        private bool IsInLoop()
        {
            return _loops.Count > 0;
        }
    }
}