using System.Collections;
using UnityEngine;

public static class DataTypeUtils
{
    public static DataType GetDataType(object value)
    {
        if (value is string)
        {
            return DataType.String;
        }

        if (value is float)
        {
            return DataType.Number;
        }

        if (value is bool)
        {
            return DataType.Boolean;
        }

        if (value is ColorHSV)
        {
            return DataType.Color;
        }

        if (value is Vector3)
        {
            return DataType.Vector;
        }

        if (value is IList)
        {
            return DataType.List;
        }

        return DataType.Any;
    }
}