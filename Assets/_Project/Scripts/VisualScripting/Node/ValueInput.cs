using UnityEngine;

public class ValueInput : Port<ValueOutput>
{
    public object DefaultValue;
    public ValueOutput Source;

    public override void Connect(ValueOutput port)
    {
        Source = port;
    }

    public T GetValue<T>()
    {
        if (Source != null)
        {
            return (T)Source.GetValue();
        }

        return (T)DefaultValue;
    }
    
    public object GetValue()
    {
        if (Source != null)
        {
            return Source.GetValue();
        }
        
        return DefaultValue;
    }
}