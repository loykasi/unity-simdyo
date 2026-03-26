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
            Chance = CreateInputValue(nameof(Chance), ScriptDataType.Single(DataType.Number)).UseInput();

            Value = CreateOutputValue(nameof(Value), GetRandom, ScriptDataType.Single(DataType.Boolean));
        }

        private ValueTransfer GetRandom()
        {
            float chance = Chance.GetValue().NumberValue;
            return ValueTransfer.CreateBool(Random.Range(0, 100) > chance);
        }
    }
}