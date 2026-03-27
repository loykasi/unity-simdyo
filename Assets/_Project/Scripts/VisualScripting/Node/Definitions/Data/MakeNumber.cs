namespace Loykas.Scripting
{
    public class MakeNumberNode : MakeVariableNode
    {
        public override DataType VariableType => DataType.Number;

        public override ScriptNode Create()
        {
            return new MakeNumberNode();
        }
    }
}