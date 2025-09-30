using UnityEngine;

namespace Loykas.Scripting
{
    [CreateAssetMenu(fileName = "SetPosition", menuName = "Scriptable Objects/Visual Scripting/Node/Set Position")]
    public class SetPosition : ScriptNodeData
    {
        public override ScriptNode Create()
        {
            return new SetPositionNode(Title);
        }
    }

    class SetPositionNode : ScriptNode
    {
        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue X;
        public InputValue Y;

        public SetPositionNode(string title) : base(title)
        {
            Enter = InputTrigger(nameof(Enter), Set);
            Exit = OutputTrigger(nameof(Exit));

            X = InputValue(nameof(X), ScriptDataType.Single(DataType.Number)).UseInput();
            Y = InputValue(nameof(Y), ScriptDataType.Single(DataType.Number)).UseInput();
        }

        public OutputTrigger Set(ScriptFlow vs)
        {
            float x = (float)X.GetValue(vs);
            float y = (float)Y.GetValue(vs);

            vs.Entity.Position = new Vector3(x, y, 0f);
            return Exit;
        }
    }
}