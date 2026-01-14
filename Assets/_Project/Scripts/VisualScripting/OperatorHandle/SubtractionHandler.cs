namespace Loykas.Scripting
{
    public class SubtractionHandler : OperatorHandler
    {
        public SubtractionHandler()
        {
            Operator<float, float>((a, b) => a - b);
            Operator<int, int>((a, b) => a - b);
            Operator<int, float>((a, b) => a - b);
            Operator<float, int>((a, b) => a - b);
        }
    }
}