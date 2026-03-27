namespace Loykas.Scripting
{
    public class MakeBooleanNode : MakeVariableNode
    {
        public override DataType VariableType => DataType.Boolean;
        
        public override ScriptNode Create()
        {
            return new MakeBooleanNode();
        }
    }
}