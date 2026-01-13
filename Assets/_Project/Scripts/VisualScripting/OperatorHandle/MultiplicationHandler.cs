namespace Loykas.Scripting
{
    public class MultiplicationHandler : OperatorHandler
    {
        public MultiplicationHandler()
        {
            Operator<float, float>((a, b) => a * b);
        }
    }
}