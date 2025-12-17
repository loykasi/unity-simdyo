using System;
using UnityEngine;

namespace Loykas.Scripting
{
    public class FunctionEnterNode : ScriptNode
    {
        public override bool ShouldIncludeInMenu => false;
        public override bool ShouldLocalized => false;

        public OutputTrigger Exit;
        public ScriptFunction Function;

        public override ScriptNode Create()
        {
            return new FunctionEnterNode();
        }

        public FunctionEnterNode()
        {
            Exit = OutputTrigger(nameof(Exit));
        }

        public override string GetNameKey()
        {
            return "Function: " + Function.Name;
        }

        public void Init(ScriptFunction function)
        {
            Function = function;

            for (int i = 0; i < ValueOutputs.Count; i++)
            {
                Flow.Disconnect(ValueOutputs[i]);
            }
            ValueOutputs.Clear();

            for (int i = 0; i < function.Inputs.Count; i++)
            {
                FunctionInput argument = function.Inputs[i];
                OutputValue output = OutputValue
                (
                    argument.Name,
                    argument.Type,
                    () => argument.Value
                ).NoLocalize();
            }

            OnNodeUpdated?.Invoke();
        }

        public OutputTrigger TriggerFunction(ScriptFlow flow)
        {
            return Exit;
        }
    }
}