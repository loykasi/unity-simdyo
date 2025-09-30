using UnityEngine;

namespace Loykas.Scripting
{
    [CreateAssetMenu(fileName = "GetAngle", menuName = "Scriptable Objects/Visual Scripting/Node/Get Angle")]
    public class GetAngle : ScriptNodeData
    {
        public override ScriptNode Create()
        {
            return new GetAngleNode(Title);
        }
    }

    class GetAngleNode : ScriptNode
    {
        public OutputValue Value;

        public GetAngleNode(string title) : base(title)
        {
            Value = OutputValue(nameof(Value), Get);
        }

        public object Get(ScriptFlow vs)
        {
            return vs.Entity.Angle;
        }
    }
}