namespace Loykas.Scripting
{
    public class AdditionHandler : OperatorHandler
    {
        public AdditionHandler()
        {
            Operator<string, string>((a, b) => a + b);
            Operator<float, float>((a, b) => a + b);
            Operator<string, float>((a, b) => a + b);
            Operator<float, string>((a, b) => a + b);
            
            Operator<int, int>((a, b) => a + b);
            Operator<int, float>((a, b) => a + b);
            Operator<float, int>((a, b) => a + b);
        }
    }
}