namespace Loykas.Scripting
{
    public class MakeStringNode : MakeVariableNode
    {
        public override DataType VariableType => DataType.String;

        public override ScriptNode Create()
        {
            return new MakeStringNode();
        }
    }
}