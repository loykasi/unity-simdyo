using UnityEngine;

namespace Loykas.Scripting
{

    class AddNode : ScriptNode
    {
        public InputValue A;
        public InputValue B;

        public OutputValue Value;

        public AddNode()
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Number)).UseInput();
            B = InputValue(nameof(B), ScriptDataType.Single(DataType.Number)).UseInput();

            Value = OutputValue(
                nameof(Value),
                (vs) =>
                {
                    return OperatorUtility.Add(A.GetValue(vs), B.GetValue(vs));
                }
            );
        }
    }
}