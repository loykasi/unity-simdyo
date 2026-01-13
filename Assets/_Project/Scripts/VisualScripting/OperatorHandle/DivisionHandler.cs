namespace Loykas.Scripting
{
    public class DivisionHandler : OperatorHandler
    {
        public DivisionHandler()
        {
            Operator<float, float>((a, b) => a / b);
        }
    }
}