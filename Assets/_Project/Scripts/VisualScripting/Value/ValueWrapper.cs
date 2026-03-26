using System;
using System.Collections;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;

namespace Loykas.Scripting
{
    // public interface IValueWrapper
    // {
    //     object ObjectValue { get; }
    // }
    
    // public class ValueWrapper<T> : IValueWrapper
    // {
    //     public object ObjectValue => Value;
    //     public T Value { get; set; }

    //     public ValueWrapper(T value = default)
    //     {
    //         Value = value;
    //     }
    // }

    public struct ValueTransfer
    {
        public ScriptDataType Type;

        public float NumberValue;
        public bool BoolValue;
        public ColorHSV ColorValue;
        public Key KeyValue;
        public int EnityIdValue;
        
        public string StringValue => (string)RefValue;
        public IList ListValue => (IList)RefValue;

        public object RefValue;

        public object GetObjectValue()
        {
            return Type.Type switch
            {
                DataType.String => StringValue,
                DataType.Number => NumberValue,
                DataType.Boolean => BoolValue,
                DataType.Color => ColorValue,
                DataType.Key => KeyValue,
                DataType.Entity => EnityIdValue,
                _ => null,
            };
        }

        public override string ToString()
        {
            return Type.Type switch
            {
                DataType.String => StringValue,
                DataType.Number => NumberValue.ToString(),
                DataType.Boolean => BoolValue.ToString(),
                DataType.Color => ColorValue.ToString(),
                DataType.Key => KeyValue.ToString(),
                DataType.Entity => EnityIdValue.ToString(),
                _ => string.Empty,
            };
        }

        public static ValueTransfer CreateString(string value)
        {
            return new ValueTransfer
            {
                Type = ScriptDataType.Single(DataType.String),
                RefValue = value  
            };
        }

        public static ValueTransfer CreateNumber(float value)
        {
            return new ValueTransfer
            {
                Type = ScriptDataType.Single(DataType.Number),
                NumberValue = value
            };
        }

        public static ValueTransfer CreateNumber(int value)
        {
            return new ValueTransfer
            {
                Type = ScriptDataType.Single(DataType.Number),
                NumberValue = value
            };
        }

        public static ValueTransfer CreateBool(bool value)
        {
            return new ValueTransfer
            {
                Type = ScriptDataType.Single(DataType.Boolean),
                BoolValue = value
            };
        }

        public static ValueTransfer CreateColor(ColorHSV value)
        {
            return new ValueTransfer
            {
                Type = ScriptDataType.Single(DataType.Color),
                ColorValue = value
            };
        }

        public static ValueTransfer CreateKey(Key value)
        {
            return new ValueTransfer
            {
                Type = ScriptDataType.Single(DataType.Key),
                KeyValue = value
            };
        }

        public static ValueTransfer CreateEntity(SceneEntity value)
        {
            return new ValueTransfer
            {
                Type = ScriptDataType.Single(DataType.Entity),
                EnityIdValue = value.Id
            };
        }

        public static ValueTransfer CreateEntity(int id)
        {
            return new ValueTransfer
            {
                Type = ScriptDataType.Single(DataType.Entity),
                EnityIdValue = id
            };
        }

        public static ValueTransfer CreateEntity(object value)
        {
            return new ValueTransfer
            {
                Type = ScriptDataType.Single(DataType.Entity),
                EnityIdValue = value == null ? -1 : (int)value
            };
        }

        public static ValueTransfer CreateList(IList value, DataType type)
        {
            return new ValueTransfer
            {
                Type = ScriptDataType.List(type),
                RefValue = value
            };
        }

        public static ValueTransfer FromValue(object value)
        {
            return value switch
            {
                string stringValue => CreateString(stringValue),
                float floatValue => CreateNumber(floatValue),
                int intValue => CreateNumber(intValue),
                bool boolValue => CreateBool(boolValue),
                ColorHSV colorValue => CreateColor(colorValue),
                SceneEntity entityValue => CreateEntity(entityValue),
                _ => default,
            };
        }

        public static ValueTransfer FromValue(DataType type, object value)
        {
            return type switch
            {
                DataType.String => CreateString((string)value),
                DataType.Number => CreateNumber((float)value),
                DataType.Boolean => CreateBool((bool)value),
                DataType.Color => CreateColor((ColorHSV)value),
                DataType.Entity => CreateEntity(value),
                DataType.Key => CreateKey((Key)value),
                _ => default,
            };
        }
    }
}
