using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loykas.Scripting
{
    public static class ValueHandler
    {
        public static object GetDefaultValue(ScriptDataType type)
        {
            if (type.IsList)
            {
                return new List<object>();
            }

            return type.Type switch
            {
                DataType.String => default(string),
                DataType.Number => default(float),
                DataType.Boolean => default(bool),
                DataType.Color => new ColorHSV(0f, 0f, 1f, 1f),
                DataType.Entity => default,
                DataType.Any => default,
                _ => default,
            };
        }

        public static void SetDefaultValue(Variable variable, ScriptDataType type)
        {
            variable.Value = GetDefaultValue(type);
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
            if (!variable.Type.IsList)
            {
                return;
            }

            IList list = (IList)variable.Value;
            list.Add(value);

            Debug.Log($"List | Add: {value}");
        }

        public static void ListEdit(Variable variable, int index, object value)
        {
            if (!variable.Type.IsList)
            {
                return;
            }

            IList list = (IList)variable.Value;
            list[index] = value;

            Debug.Log($"List | Edit: {value}");
        }

        public static void ListRemoveAt(Variable variable, int index)
        {
            if (!variable.Type.IsList)
            {
                return;
            }

            IList list = (IList)variable.Value;
            list.RemoveAt(index);

            Debug.Log($"List | Remove at: {index}");
        }
    }
}