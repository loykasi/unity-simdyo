using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class QuaternionConverter : JsonConverter<Quaternion>
{
    public override void WriteJson(JsonWriter writer, Quaternion value, Newtonsoft.Json.JsonSerializer serializer)
    {
        JObject obj = new JObject
        (
            new JProperty("x", value.x),
            new JProperty("y", value.y),
            new JProperty("z", value.z),
            new JProperty("w", value.w)
        );

        obj.WriteTo(writer);
    }

    public override Quaternion ReadJson(JsonReader reader, Type objectType, Quaternion existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
        JObject obj = JObject.Load(reader);
        return new Quaternion
        (
            obj.Value<float>("x"),
            obj.Value<float>("y"),
            obj.Value<float>("z"),
            obj.Value<float>("w")
        );
    }
}