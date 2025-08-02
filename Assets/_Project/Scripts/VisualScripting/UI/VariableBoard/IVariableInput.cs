public interface IVariableInput
{
    void SetValue(object value);
    void Enable();
    void Disable();
    object GetValue();
}