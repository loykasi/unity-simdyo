using UnityEngine;

namespace Loykas.Scripting
{
    class SetAngleNode : ScriptNode
    {
        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue Value;

        public SetAngleNode(string title) : base(title)
        {
            Enter = InputTrigger(nameof(Enter), Set);
            Exit = OutputTrigger(nameof(Exit));

            Value = InputValue(nameof(Value), ScriptDataType.Single(DataType.Number)).UseInput();
        }

        public OutputTrigger Set(ScriptFlow vs)
        {
            float angle = (float)Value.GetValue(vs);

            vs.Entity.Angle = angle;
            return Exit;
        }
    }
}