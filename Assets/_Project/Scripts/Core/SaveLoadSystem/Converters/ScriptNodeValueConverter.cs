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

        JToken token = obj["Value"];
        
        switch (type)
        {
            case DataType.String:
                value.Value = token?.ToObject<string>(serializer);
                break;
            case DataType.Number:
                value.Value = token.ToObject<float>(serializer);
                break;
            case DataType.Boolean:
                value.Value = token.ToObject<bool>(serializer);
                break;
            case DataType.Color:
                value.Value = token.ToObject<ColorHSV>(serializer);
                break;
            case DataType.Key:
                value.Value = new Key(token.ToObject<string>(serializer));
                break;
            case DataType.Entity:
                value.Value = token.Type == JTokenType.Null ? null : token.ToObject<int>(serializer);
                Debug.Log(value.GetType());
                Debug.Log(value.Value);
                break;
        }

        return value;
    }
}