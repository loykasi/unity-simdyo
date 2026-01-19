using UnityEngine;

namespace Loykas.Scripting
{
    class ClampNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Operator;

        public InputValue Value;
        public InputValue Min;
        public InputValue Max;

        public OutputValue Output;

        public override ScriptNode Create()
        {
            return new ClampNode();
        }

        public ClampNode()
        {
            Value = InputValue(nameof(Value), ScriptDataType.Single(DataType.Number))
                .UseInput()
                .HideLabel();

            Min = InputValue(nameof(Min), ScriptDataType.Single(DataType.Number))
                .UseInput()
                .NoLocalize();
            
            Max = InputValue(nameof(Max), ScriptDataType.Single(DataType.Number))
                .UseInput()
                .NoLocalize();

            Output = OutputValue(nameof(Output), ScriptDataType.Single(DataType.Number), Get).HideLabel();
        }

        private object Get()
        {
            float value = (float)Value.GetValue();
            float min = (float)Min.GetValue();
            float max = (float)Max.GetValue();
            return Mathf.Clamp(value, min, max);
        }
    }
}