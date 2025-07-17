public class MultiplicationHandler : OperatorHandler
{
    public MultiplicationHandler()
    {
        Operator<double, double>((a, b) => a * b);
    }
}