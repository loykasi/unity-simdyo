using UnityEngine;

namespace Loykas.Scripting
{
    [CreateAssetMenu(fileName = "MakeString", menuName = "Scriptable Objects/Visual Scripting/Node/Make String")]
    public class MakeString : MakeVariable
    {
        public override ScriptDataType Type => ScriptDataType.Single(DataType.String);
    }
}