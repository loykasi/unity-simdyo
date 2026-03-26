namespace Loykas.Scripting
{    
    public class ScriptPool : Singleton<ScriptPool>
    {
        public VariablePool Variable = new();
        public FunctionPool Function = new();
        public NodeTaskPool NodeTask = new();
        public ConnectionPool Connection = new();
    }
}
