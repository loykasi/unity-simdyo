namespace Loykas.Scripting
{
    public class ModuloHandler : OperatorHandler
    {
        public ModuloHandler()
        {
            Operator<float, float>((a, b) => a % b);
        }
    }
}