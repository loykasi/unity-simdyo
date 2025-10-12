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

        private ScriptFunction _function;

        public FunctionCallNode()
        {
            Enter = InputTrigger(nameof(Enter), TriggerFunction);
            Exit = OutputTrigger(nameof(Exit));
        }

        public void Init(ScriptFunction function)
        {
            _function = function;

            ValueInputs.Clear();
            for (int i = 0; i < function.Inputs.Count; i++)
            {
                FunctionInput argument = function.Inputs[i];
                InputValue input = InputValue(argument.Name, argument.Type);
            }
        }

        public OutputTrigger TriggerFunction(ScriptFlow flow)
        {
            return _function.StartNode.Exit;
        }
    }
}