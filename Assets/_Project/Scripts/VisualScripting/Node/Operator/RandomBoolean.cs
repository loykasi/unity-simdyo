using UnityEngine;

namespace Loykas.Scripting
{
    class RandomBooleanNode : ScriptNode
    {
        public InputValue Chance;

        public OutputValue Value;

        public RandomBooleanNode()
        {
            Chance = InputValue(nameof(Chance), ScriptDataType.Single(DataType.Number)).UseInput();

            Value = OutputValue(nameof(Value), GetRandom);
        }

        private object GetRandom()
        {
            float chance = (float)Chance.GetValue();
            return Random.Range(0, 100) > chance;
        }
    }
}