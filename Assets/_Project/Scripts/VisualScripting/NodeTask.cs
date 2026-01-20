using System;
using System.Collections.Generic;
using UnityEngine;

namespace Loykas.Scripting
{
    public class NodeTask
    {
        public bool IsDone;
        public bool ShouldExecuteNextFrame;
        public bool ShouldRemove;

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

        public void Stop()
        {
            Trigger = null;
        }

        public void Invoke(ScriptFlow flow)
        {
            try
            {
                IsDone = false;
                while (Trigger != null)
                {
                    bool isDone = Trigger.Invoke(this);

                    if (!isDone)
                    {
                        ShouldExecuteNextFrame = true;
                        return;
                    }

                    if (Trigger == null)
                    {
                        Remove();
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
                                Remove();
                            }

                            IsDone = true;
                            return;
                        }
                    }
                }   
            }
            catch (System.Exception exception)
            {
                Remove();
                Debug.LogWarning(exception);
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

        private void Remove()
        {
            ShouldRemove = true;
        }
    }
}