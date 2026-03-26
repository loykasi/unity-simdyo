using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loykas.Scripting
{
    public static class ValueHandler
    {
        public static ValueTransfer GetDefaultValueWrapper(ScriptDataType type)
        {
            switch (type.Kind)
            {
                case DataKind.Simple:
                {
                    return type.Type switch
                    {
                        DataType.String => ValueTransfer.CreateString(default),
                        DataType.Number => ValueTransfer.CreateNumber(default),
                        DataType.Boolean => ValueTransfer.CreateBool(default),
                        DataType.Color => ValueTransfer.CreateColor(new ColorHSV(0f, 0f, 1f, 1f)),
                        DataType.Key => ValueTransfer.CreateKey(new Key(KeyCode.Any)),
                        _ => default,
                    };
                };
                case DataKind.List:
                {
                    return type.Type switch
                    {
                        DataType.String => ValueTransfer.CreateList(new List<string>(), DataType.String),
                        DataType.Number => ValueTransfer.CreateList(new List<float>(), DataType.Number),
                        DataType.Boolean => ValueTransfer.CreateList(new List<bool>(), DataType.Boolean),
                        DataType.Color => ValueTransfer.CreateList(new List<ColorHSV>(), DataType.Color),
                        _ => default,
                    };
                }
                default:
                    return default;
            }
        }

        public static object GetDefaultValue(ScriptDataType type)
        {
            switch (type.Kind)
            {
                case DataKind.Simple:
                {
                    return type.Type switch
                    {
                        DataType.String => default(string),
                        DataType.Number => default(float),
                        DataType.Boolean => default(bool),
                        DataType.Color => new ColorHSV(0f, 0f, 1f, 1f),
                        DataType.Key => new Loykas.Scripting.Key(Loykas.Scripting.KeyCode.Any),
                        _ => default,
                    };
                };
                case DataKind.List:
                {
                    return type.Type switch
                    {
                        DataType.String => new List<string>(),
                        DataType.Number => new List<float>(),
                        DataType.Boolean => new List<bool>(),
                        DataType.Color => new List<ColorHSV>(),
                        _ => default,
                    };
                }
                default:
                    return default;
            }
        }

        public static void SetDefaultValue(ScriptFlow flow, Variable variable, ScriptDataType type)
        {
            variable.Value = GetDefaultValue(type);
            variable.SetType(type);

            Debug.Log($"Set Default Value: {variable.Value}");
        }

        public static void SetValue(Variable variable, object value)
        {
            variable.Value = value;
            Debug.Log($"Set Value: {variable.Value}");
        }

        public static void ListAdd(Variable variable, object value)
        {
            if (variable.Type.Kind != DataKind.List)
            {
                return;
            }

            IList list = (IList)variable.Value;
            list.Add(value);

            Debug.Log($"List | Add: {value}");
        }

        public static void ListEdit(Variable variable, int index, object value)
        {
            if (variable.Type.Kind != DataKind.List)
            {
                return;
            }

            IList list = (IList)variable.Value;
            list[index] = value;

            Debug.Log($"List | Edit: {value}");
        }

        public static void ListRemoveAt(Variable variable, int index)
        {
            if (variable.Type.Kind != DataKind.List)
            {
                return;
            }

            IList list = (IList)variable.Value;
            list.RemoveAt(index);

            Debug.Log($"List | Remove at: {index}");
        }
    }
}