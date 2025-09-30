using UnityEngine;

namespace Loykas.Scripting
{
    [CreateAssetMenu(fileName = "RandomBoolean", menuName = "Scriptable Objects/Visual Scripting/Node/RandomBoolean")]
    public class RandomBoolean : ScriptNodeData
    {
        public string InputA;
        public string InputB;

        public override ScriptNode Create()
        {
            return new RandomBooleanNode(Title);
        }
    }

    class RandomBooleanNode : ScriptNode
    {
        public InputValue Chance;

        public OutputValue Value;

        public RandomBooleanNode(string title) : base(title)
        {
            Chance = InputValue(nameof(Chance), ScriptDataType.Single(DataType.Number)).UseInput();

            Value = OutputValue(nameof(Value), GetRandom);
        }

        private object GetRandom(ScriptFlow flow)
        {
            float chance = (float)Chance.GetValue(flow);
            return Random.Range(0, 100) > chance;
        }
    }
}