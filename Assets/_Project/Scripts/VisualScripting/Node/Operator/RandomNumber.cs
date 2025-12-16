using UnityEngine;

namespace Loykas.Scripting
{
    class RandomNumberNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Operator;

        public InputValue A;
        public InputValue B;

        public OutputValue Value;

        public override ScriptNode Create()
        {
            return new RandomNumberNode();
        }

        public RandomNumberNode()
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Number)).UseInput();
            B = InputValue(nameof(B), ScriptDataType.Single(DataType.Number)).UseInput();

            Value = OutputValue(nameof(Value), ScriptDataType.Single(DataType.Number), GetRandom).HideLabel();
        }

        private object GetRandom()
        {
            float a = (float)A.GetValue();
            float b = (float)B.GetValue();
            return Random.Range(a, b);
        }
    }
}