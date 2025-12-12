using System;
using System.Collections.Generic;
using Loykas.Scripting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class ScriptNodeValueConverter : JsonConverter<ScriptNodeValueData>
{
    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, ScriptNodeValueData value, Newtonsoft.Json.JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override ScriptNodeValueData ReadJson(
        JsonReader reader,
        Type objectType,
        ScriptNodeValueData existingValue,
        bool hasExistingValue,
        Newtonsoft.Json.JsonSerializer serializer)
    {
        JObject obj = JObject.Load(reader);

        string key = obj["Key"].ToObject<string>(serializer);
        DataType type = obj["Type"].ToObject<DataType>(serializer);
        
        ScriptNodeValueData value = new()
        {
            Key = key,
            Type = type,
        };
        
        switch (type)
        {
            case DataType.String:
            case DataType.Entity:
                value.Value = obj["Value"].ToObject<string>(serializer);
                break;
            case DataType.Number:
                value.Value = obj["Value"].ToObject<float>(serializer);
                break;
            case DataType.Boolean:
                value.Value = obj["Value"].ToObject<bool>(serializer);
                break;
            case DataType.Color:
                value.Value = obj["Value"].ToObject<ColorHSV>(serializer);
                break;
        }

        return value;
    }
}