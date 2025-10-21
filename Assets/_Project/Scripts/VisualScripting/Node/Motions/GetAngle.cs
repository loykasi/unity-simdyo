using UnityEngine;

namespace Loykas.Scripting
{

    class GetAngleNode : ScriptNode
    {
        public OutputValue Value;

        public GetAngleNode()
        {
            Value = OutputValue(nameof(Value), Get);
        }

        public object Get(ScriptFlow vs)
        {
            return vs.Entity.Angle;
        }
    }
}