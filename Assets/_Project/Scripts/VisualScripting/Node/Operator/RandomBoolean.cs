using UnityEngine;

namespace Loykas.Scripting
{
    class RandomBooleanNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Operator;

        public InputValue Chance;

        public OutputValue Value;

        public override ScriptNode Create()
        {
            return new RandomBooleanNode();
        }

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