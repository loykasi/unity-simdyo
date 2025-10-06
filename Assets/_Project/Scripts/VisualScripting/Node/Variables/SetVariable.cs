using UnityEngine;

namespace Loykas.Scripting
{
    [ScriptNode(ScriptNodeCategory.Data)]
    public class SetVariableNodeContent : ScriptNodeContent
    {
        public override System.Type Type => typeof(SetVariableNode);
        public override ScriptNode Create() => new SetVariableNode();
    }
    
    public class SetVariableNode : ScriptNode
    {
        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue Variable;
        public InputValue Value;

        public SetVariableNode() : base()
        {
            Enter = InputTrigger(nameof(Enter), Set);
            Exit = OutputTrigger(nameof(Exit));

            Variable = InputValue(nameof(Variable))
                            .UseVariableInput()
                            .DisableConnection()
                            .HideLabel();

            Value = InputValue(nameof(Value));
        }

        private OutputTrigger Set(ScriptFlow vs)
        {
            string name = Variable.GetValue(vs).ToString();
            object value = Value.GetValue(vs);
            vs.UpdateVariable(name, value);
            return Exit;
        }
    }
}