using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Loykas.Scripting;
using System.Collections.Generic;

public class VariableConverter : JsonConverter<ScriptVariableSaveData>
{
    public override bool CanWrite => false;

    public override ScriptVariableSaveData ReadJson(JsonReader reader, Type objectType, ScriptVariableSaveData existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
        JObject obj = JObject.Load(reader);

        string name = obj["Name"].ToObject<string>(serializer);
        DataType type = obj["Type"].ToObject<DataType>(serializer);
        DataKind kind = obj["Kind"].ToObject<DataKind>(serializer);
        
        ScriptVariableSaveData variable = new()
        {
            Name = name,
            Type = type,
            Kind = kind
        };

        switch (kind)
        {
            case DataKind.Simple:
            {
                switch (type)
                {
                    case DataType.String:
                        variable.Value = obj["Value"].ToObject<string>(serializer);
                        break;
                    case DataType.Number:
                        variable.Value = obj["Value"].ToObject<float>(serializer);
                        break;
                    case DataType.Boolean:
                        variable.Value = obj["Value"].ToObject<bool>(serializer);
                        break;
                    case DataType.Color:
                        variable.Value = obj["Value"].ToObject<ColorHSV>(serializer);
                        break;
                }
                break;
            };
            case DataKind.List:
            {
                switch (type)
                {
                    case DataType.String:
                        variable.Value = obj["Value"].ToObject<IList<string>>(serializer);
                        break;
                    case DataType.Number:
                        variable.Value = obj["Value"].ToObject<IList<float>>(serializer);
                        break;
                    case DataType.Boolean:
                        variable.Value = obj["Value"].ToObject<IList<bool>>(serializer);
                        break;
                    case DataType.Color:
                        variable.Value = obj["Value"].ToObject<IList<ColorHSV>>(serializer);
                        break;
                }
                break;
            }
            default:
                break;
        }            

        return variable;
        // throw new NotImplementedException();
    }

    public override void WriteJson(JsonWriter writer, ScriptVariableSaveData value, Newtonsoft.Json.JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}