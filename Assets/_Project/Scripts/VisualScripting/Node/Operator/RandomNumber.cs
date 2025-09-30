using UnityEngine;

namespace Loykas.Scripting
{
    [CreateAssetMenu(fileName = "RandomNumber", menuName = "Scriptable Objects/Visual Scripting/Node/RandomNumber")]
    public class RandomNumber : ScriptNodeData
    {
        public string InputA;
        public string InputB;

        public override ScriptNode Create()
        {
            return new RandomNumberNode(Title);
        }
    }

    class RandomNumberNode : ScriptNode
    {
        public InputValue A;
        public InputValue B;

        public OutputValue Value;

        public RandomNumberNode(string title) : base(title)
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Number)).UseInput();
            B = InputValue(nameof(B), ScriptDataType.Single(DataType.Number)).UseInput();

            Value = OutputValue(nameof(Value), GetRandom);
        }

        private object GetRandom(ScriptFlow flow)
        {
            float a = (float)A.GetValue(flow);
            float b = (float)B.GetValue(flow);
            return Random.Range(a, b);
        }
    }
}