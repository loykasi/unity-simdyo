using UnityEngine;

namespace Loykas.Scripting
{
    [CreateAssetMenu(fileName = "MakeBoolean", menuName = "Scriptable Objects/Visual Scripting/Node/Make Boolean")]
    public class MakeBoolen : MakeVariable
    {
        public override ScriptDataType Type => ScriptDataType.Single(DataType.Boolean);
    }
}