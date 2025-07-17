public class ModuloHandler : OperatorHandler
{
    public ModuloHandler()
    {
        Operator<double, double>((a, b) => a % b);
    }
}