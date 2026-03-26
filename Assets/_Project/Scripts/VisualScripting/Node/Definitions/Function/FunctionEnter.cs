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
            Exit = CreateOutputTrigger(nameof(Exit), PortSettings.Default);
        }

        public override string GetNameKey()
        {
            return Function != null ? Function.Name : string.Empty;
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
                OutputValue output = CreateOutputValue
                (
                    argument.Name,
                    () => ValueTransfer.FromValue(argument.Value),
                    argument.Type,
                    new PortSettings
                    {
                        IsLocalizationDisabled = true
                    }
                );
            }

            OnNodeUpdated?.Invoke();
        }

        public OutputTrigger TriggerFunction(ScriptFlow flow)
        {
            return Exit;
        }
    }
}