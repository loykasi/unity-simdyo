using UnityEngine;

namespace Loykas.Scripting
{
    class LessNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Operator;

        public InputValue A;
        public InputValue B;

        public OutputValue Output;

        public override ScriptNode Create()
        {
            return new LessNode();
        }

        public LessNode()
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Number))
                .UseInput()
                .NoLocalize();
            
            B = InputValue(nameof(B), ScriptDataType.Single(DataType.Number))
                .UseInput()
                .NoLocalize();

            Output = OutputValue(nameof(Output), ScriptDataType.Single(DataType.Boolean), Get).HideLabel();
        }

        private object Get() => A.GetValue<float>() < B.GetValue<float>();
    }
}