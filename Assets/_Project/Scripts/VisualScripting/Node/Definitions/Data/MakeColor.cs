namespace Loykas.Scripting
{
    public class MakeColorNode : MakeVariableNode
    {
        public override DataType VariableType => DataType.Color;

        public override ScriptNode Create()
        {
            return new MakeColorNode();
        }
    }
}