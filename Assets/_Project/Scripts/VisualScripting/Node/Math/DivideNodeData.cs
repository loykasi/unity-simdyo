using UnityEngine;

namespace Loykas.Scripting
{
[CreateAssetMenu(fileName = "DivideNode", menuName = "Scriptable Objects/Visual Scripting/Node/Divide")]
public class DivideNodeData : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new DivideNode(Title);
    }
}

    class DivideNode : ScriptNode
    {
        public InputValue A;
        public InputValue B;

        public OutputValue Output;

        public DivideNode(string title) : base(title)
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Number)).UseInput();
            B = InputValue(nameof(B), ScriptDataType.Single(DataType.Number)).UseInput();

            Output = OutputValue(
                nameof(Output),
                (vs) =>
                {
                    return OperatorUtility.Divide(A.GetValue(vs), B.GetValue(vs));
                }
            );
        }
    }
}