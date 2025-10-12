namespace Loykas.Scripting
{
    public class FunctionInput
    {
        public string Name;
        public ScriptDataType Type = ScriptDataType.Single(DataType.Any);
        public object Value;
    }
}