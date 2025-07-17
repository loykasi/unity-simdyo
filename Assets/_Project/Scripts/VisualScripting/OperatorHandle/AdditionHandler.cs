public class AdditionHandler : OperatorHandler
{
    public AdditionHandler()
    {
        Operator<string, string>((a, b) => a + b);
        Operator<double, double>((a, b) => a + b);
        Operator<string, double>((a, b) => a + b);
        Operator<double, string>((a, b) => a + b);
    }
}