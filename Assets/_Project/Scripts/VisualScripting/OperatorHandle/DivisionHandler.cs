namespace Loykas.Scripting
{
    public class DivisionHandler : OperatorHandler
    {
        public DivisionHandler()
        {
            Operator<double, double>((a, b) => a / b);
        }
    }
}