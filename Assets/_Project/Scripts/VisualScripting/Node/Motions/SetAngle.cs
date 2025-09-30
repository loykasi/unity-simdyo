using UnityEngine;

namespace Loykas.Scripting
{
    [CreateAssetMenu(fileName = "SetAngle", menuName = "Scriptable Objects/Visual Scripting/Node/Set Angle")]
    public class SetAngle : ScriptNodeData
    {
        public override ScriptNode Create()
        {
            return new SetAngleNode(Title);
        }
    }

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