using System;
using System.Collections.Generic;
using UnityEngine;

namespace Loykas.Scripting
{
    [ScriptNode(ScriptNodeCategory.Data)]
    public class FunctionEnterContent : ScriptNodeContent
    {
        public override Type Type => typeof(FunctionEnterNode);
        public override ScriptNode Create() => new FunctionEnterNode();
    }

    public class FunctionEnterNode : ScriptNode
    {
        public InputTrigger Enter;
        public OutputTrigger Exit;

        public FunctionEnterNode()
        {
            Enter = InputTrigger(nameof(Enter), TriggerFunction);
            Exit = OutputTrigger(nameof(Exit)).HideLabel();
        }

        public void Init(ScriptFunction function)
        {
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
                    (flow) => argument.Value
                );
            }

            OnNodeUpdated?.Invoke();
            Debug.Log($"Update start node: output - {ValueOutputs.Count}");
        }

        public OutputTrigger TriggerFunction(ScriptFlow flow)
        {
            return Exit;
        }
    }
}