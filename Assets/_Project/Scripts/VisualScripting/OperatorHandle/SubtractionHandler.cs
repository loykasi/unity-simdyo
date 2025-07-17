public class SubtractionHandler : OperatorHandler
{
    public SubtractionHandler()
    {
        Operator<double, double>((a, b) => a - b);
    }
}