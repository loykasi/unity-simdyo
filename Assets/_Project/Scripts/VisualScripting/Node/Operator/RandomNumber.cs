using UnityEngine;

namespace Loykas.Scripting
{

    class RandomNumberNode : ScriptNode
    {
        public InputValue A;
        public InputValue B;

        public OutputValue Value;

        public RandomNumberNode()
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Number)).UseInput();
            B = InputValue(nameof(B), ScriptDataType.Single(DataType.Number)).UseInput();

            Value = OutputValue(nameof(Value), GetRandom);
        }

        private object GetRandom()
        {
            float a = (float)A.GetValue();
            float b = (float)B.GetValue();
            return Random.Range(a, b);
        }
    }
}