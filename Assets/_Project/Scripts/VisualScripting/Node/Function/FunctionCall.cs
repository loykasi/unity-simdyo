using System;
using System.Collections.Generic;
using UnityEngine;

namespace Loykas.Scripting
{
    [ScriptNode(ScriptNodeCategory.Data)]
    public class FunctionCallContent : ScriptNodeContent
    {
        public override Type Type => typeof(FunctionCallNode);
        public override ScriptNode Create() => new FunctionCallNode();
    }

    public class FunctionCallNode : ScriptNode
    {
        public InputTrigger Enter;
        public OutputTrigger Exit;

        public ScriptFunction Function;

        private bool _firstRun = true;
        private NodeTask _task = new();

        public FunctionCallNode()
        {
            Enter = InputTrigger(nameof(Enter), TriggerFunction);
            Exit = OutputTrigger(nameof(Exit)).HideLabel();
        }

        public override string GetNameKey()
        {
            return Function.Name;
        }

        public void Init(ScriptFunction function)
        {
            Function = function;

            Function.OnUpdated += OnFunctionUpdated;

            OnFunctionUpdated();
        }

        public OutputTrigger TriggerFunction(ScriptFlow flow)
        {
            if (_firstRun)
            {
                _firstRun = false;

                _task.From = Function.StartNode.Exit;
                _task.Trigger = Function.StartNode.Exit.Invoke(flow);
            }

            for (int i = 0; i < Function.Inputs.Count; i++)
            {
                FunctionInput argument = Function.Inputs[i];
                InputValue input = ValueInputs[i];
                argument.Value = input.GetValue(flow);
            }

            _task.Invoke(flow);
            if (!_task.IsDone)
            {
                return null;
            }

            _firstRun = true;
            return Exit;
        }

        private void OnFunctionUpdated()
        {
            for (int i = 0; i < ValueInputs.Count; i++)
            {
                Flow.Disconnect(ValueInputs[i]);
            }
            ValueInputs.Clear();

            for (int i = 0; i < Function.Inputs.Count; i++)
            {
                FunctionInput argument = Function.Inputs[i];
                InputValue input = InputValue
                (
                    argument.Name,
                    argument.Type
                );
            }

            OnNodeUpdated?.Invoke();
        }
    }
}