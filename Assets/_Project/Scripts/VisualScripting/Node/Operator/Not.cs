using UnityEngine;

namespace Loykas.Scripting
{
[CreateAssetMenu(fileName = "Not", menuName = "Scriptable Objects/Visual Scripting/Node/Not")]
public class Not : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new NotNode(Title);
    }
}

    class NotNode : ScriptNode
    {
        public InputValue A;

        public OutputValue Output;

        public NotNode(string title) : base(title)
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Boolean)).UseInput();

            Output = OutputValue(
                nameof(Output),
                (vs) =>
                {
                    return ! A.GetValue<bool>(vs);
                }
            );
        }
    }
}