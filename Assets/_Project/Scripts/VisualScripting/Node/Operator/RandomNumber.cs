using UnityEngine;

namespace Loykas.Scripting
{
    class RandomNumberNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Operator;

        public InputValue Min;
        public InputValue Max;
        public InputValue IntegersOnly;

        public OutputValue Value;

        public override ScriptNode Create()
        {
            return new RandomNumberNode();
        }

        public RandomNumberNode()
        {
            Min = InputValue(nameof(Min), ScriptDataType.Single(DataType.Number))
                .UseInput()
                .NoLocalize();
            
            Max = InputValue(nameof(Max), ScriptDataType.Single(DataType.Number))
                .UseInput()
                .NoLocalize();

            IntegersOnly = InputValue(nameof(IntegersOnly), ScriptDataType.Single(DataType.Boolean))
                .UseInput()
                .DisableConnection()
                .NoLocalize();

            Value = OutputValue(nameof(Value), ScriptDataType.Single(DataType.Number), GetRandom).HideLabel();
        }

        private object GetRandom()
        {
            float a = (float)Min.GetValue();
            float b = (float)Max.GetValue();
            
            bool isInteger = (bool)IntegersOnly.GetValue();
            if (isInteger)
            {
                return (float)Random.Range((int)a, (int)b + 1);
            }
            return Random.Range(a, b);
        }
    }
}