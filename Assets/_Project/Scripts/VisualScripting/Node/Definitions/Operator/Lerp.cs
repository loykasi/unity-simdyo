using UnityEngine;

namespace Loykas.Scripting
{
    class LerpNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Operator;

        public InputValue A;
        public InputValue B;
        public InputValue T;

        public OutputValue Output;

        public override ScriptNode Create()
        {
            return new LerpNode();
        }

        public LerpNode()
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Number))
                .UseInput()
                .NoLocalize();
            
            B = InputValue(nameof(B), ScriptDataType.Single(DataType.Number))
                .UseInput()
                .NoLocalize();

            T = InputValue(nameof(T), ScriptDataType.Single(DataType.Number))
                .UseInput()
                .NoLocalize();

            Output = OutputValue(nameof(Output), ScriptDataType.Single(DataType.Number), Get).HideLabel();
        }

        private object Get()
        {
            float t = (float)T.GetValue();
            float a = (float)A.GetValue();
            float b = (float)B.GetValue();
            return Mathf.Lerp(a, b, t);
        }
    }
}