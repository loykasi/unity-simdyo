using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class VariableConverter : JsonConverter<Variable>
{
    public override bool CanWrite => false;

    public override Variable ReadJson(JsonReader reader, Type objectType, Variable existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
        JObject obj = JObject.Load(reader);

        DataType type = obj["Type"].ToObject<DataType>(serializer);
        // ListType? subType = obj["SubType"].ToObject<ListType?>(serializer);
        // Variable variable = new()
        // {
        //     Type = type,
        //     SubType = subType
        // };

        // switch (type)
        // {
        //     case DataType.String:
        //         variable.Value = obj["Value"].ToObject<string>(serializer);
        //         break;
        //     case DataType.Number:
        //         variable.Value = obj["Value"].ToObject<float>(serializer);
        //         break;
        //     case DataType.Boolean:
        //         variable.Value = obj["Value"].ToObject<bool>(serializer);
        //         break;
        //     case DataType.Vector:
        //         Debug.Log("vector 3");
        //         variable.Value = obj["Value"].ToObject<Vector3>(serializer);
        //         break;
        //     case DataType.Color:
        //         variable.Value = obj["Value"].ToObject<ColorHSV>(serializer);
        //         break;
        //     case DataType.List:
        //         variable.Value = subType switch
        //         {
        //             ListType.String => obj["Value"].ToObject<IList<string>>(serializer),
        //             ListType.Number => obj["Value"].ToObject<IList<float>>(serializer),
        //             ListType.Boolean => obj["Value"].ToObject<IList<bool>>(serializer),
        //             _ => throw new ArgumentOutOfRangeException(),
        //         };
        //         break;
        // }

        // return variable;
        return null;
    }

    public override void WriteJson(JsonWriter writer, Variable value, Newtonsoft.Json.JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}