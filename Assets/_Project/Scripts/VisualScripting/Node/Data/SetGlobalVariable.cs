using UnityEngine;

namespace Loykas.Scripting
{    
    public class SetGlobalVariableNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Data;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue Variable;
        public InputValue Value;

        public override ScriptNode Create()
        {
            return new SetGlobalVariableNode();
        }

        public SetGlobalVariableNode() : base()
        {
            Enter = CreateInputTrigger(nameof(Enter), Set);
            Exit = OutputTrigger(nameof(Exit));

            Variable = InputValue(nameof(Variable), ScriptDataType.Single(DataType.String))
                            .UseGlobalVariableInput()
                            .DisableConnection()
                            .HideLabel();
                            
            Value = InputValue(nameof(Value));
        }

        private OutputTrigger Set()
        {
            string name = Variable.GetValue().ToString();
            object value = Value.GetValue();
            Flow.UpdateVariable(name, value);
            return Exit;
        }
    }
}