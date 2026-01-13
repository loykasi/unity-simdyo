namespace Loykas.Scripting
{
    public class SubtractionHandler : OperatorHandler
    {
        public SubtractionHandler()
        {
            Operator<float, float>((a, b) => a - b);
        }
    }
}