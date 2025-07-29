public class Variable
{
    public DataType Type;
    public object Value;

    public Variable(DataType type, object value)
    {
        Type = type;
        Value = value;
    }
}