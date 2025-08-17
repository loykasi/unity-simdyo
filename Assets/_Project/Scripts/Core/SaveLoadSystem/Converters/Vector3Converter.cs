using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class Vector3Converter : JsonConverter<Vector3>
{
    public override void WriteJson(JsonWriter writer, Vector3 value, Newtonsoft.Json.JsonSerializer serializer)
    {
        Debug.Log("Serialize vector3");
        JObject obj = new JObject
        (
            new JProperty("x", value.x),
            new JProperty("y", value.y),
            new JProperty("z", value.z)
        );

        obj.WriteTo(writer);
    }

    public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
        Debug.Log("Deserialize vector3");
        JObject obj = JObject.Load(reader);
        return new Vector3
        (
            obj.Value<float>("x"),
            obj.Value<float>("y"),
            obj.Value<float>("z")
        );
    }
}