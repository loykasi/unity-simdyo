using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ValueHandler
{
    public static void SetDefaultValue(Variable variable, DataType type)
    {
        if (type == DataType.Any)
        {
            return;
        }

        variable.Value = type switch
        {
            DataType.String => default(string),
            DataType.Number => default(float),
            DataType.Boolean => default(bool),
            DataType.Vector => default(Vector3),
            DataType.Color => new ColorHSV(0f, 0f, 1f, 1f),
            DataType.Entity => null,
            DataType.List => new List<object>(),
        };

        variable.Type = type;
        
        Debug.Log($"Set Default Value: {variable.Value}");
    }

    public static void SetValue(Variable variable, object value)
    {
        variable.Value = value;
        Debug.Log($"Set Value: {variable.Value}");
    }

    public static void ListAdd(Variable variable, object value)
    {
        if (variable.Type != DataType.List)
        {
            return;
        }

        IList list = (IList)variable.Value;
        list.Add(value);

        Debug.Log($"List | Add: {value}");
    }

    public static void ListEdit(Variable variable, int index, object value)
    {
        if (variable.Type != DataType.List)
        {
            return;
        }

        IList list = (IList)variable.Value;
        list[index] = value;

        Debug.Log($"List | Edit: {value}");
    }

    public static void ListRemoveAt(Variable variable, int index)
    {
        if (variable.Type != DataType.List)
        {
            return;
        }

        IList list = (IList)variable.Value;
        list.RemoveAt(index);

        Debug.Log($"List | Remove at: {index}");
    }
}