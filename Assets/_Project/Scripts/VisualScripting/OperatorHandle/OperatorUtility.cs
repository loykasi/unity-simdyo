namespace Loykas.Scripting
{
    public static class OperatorUtility
    {
        private static readonly AdditionHandler _additionHandler = new();
        private static readonly SubtractionHandler _subtractionHandler = new();
        private static readonly MultiplicationHandler _multiplicationHandler = new();
        private static readonly DivisionHandler _divisionHandler = new();
        private static readonly ModuloHandler _moduloHandler = new();
        private static readonly EqualHandler _equalHandler = new();

        public static object Add(object a, object b)
        {
            return _additionHandler.Operate(a, b);
        }

        public static object Subtract(object a, object b)
        {
            return _subtractionHandler.Operate(a, b);
        }

        public static object Multiply(object a, object b)
        {
            return _multiplicationHandler.Operate(a, b);
        }

        public static object Divide(object a, object b)
        {
            return _divisionHandler.Operate(a, b);
        }

        public static object Modulo(object a, object b)
        {
            return _moduloHandler.Operate(a, b);
        }

        public static object Equal(object a, object b)
        {
            return _equalHandler.Operate(a, b);
        }
    }
}