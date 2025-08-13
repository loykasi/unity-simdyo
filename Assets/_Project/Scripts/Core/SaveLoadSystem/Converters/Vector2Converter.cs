using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class Vector2Converter : JsonConverter<Vector2>
{
    public override void WriteJson(JsonWriter writer, Vector2 value, Newtonsoft.Json.JsonSerializer serializer)
    {
        JObject obj = new JObject
        (
            new JProperty("x", value.x),
            new JProperty("y", value.y)
        );

        obj.WriteTo(writer);
    }

    public override Vector2 ReadJson(JsonReader reader, Type objectType, Vector2 existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
        JObject obj = JObject.Load(reader);
        return new Vector2
        (
            obj.Value<float>("x"),
            obj.Value<float>("y")
        );
    }
}