public interface IVariableInput
{
    DataType Type { get; }
    void SetValue(object value);
    void Enable();
    void Disable();
    object GetValue();
}