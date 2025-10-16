using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class ScriptNodeConverter : JsonConverter<ScriptNodeSaveData>
{
    public override bool CanWrite => false;

    public override ScriptNodeSaveData ReadJson(JsonReader reader, Type objectType, ScriptNodeSaveData existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
        var obj = JObject.Load(reader);

        string type = obj["Type"].ToString();

        ScriptNodeSaveData nodeData = type switch
        {
            "FunctionEnter" => new ScriptNodeFunctionSaveData(),
            "FunctionCall" => new ScriptNodeFunctionSaveData(),
            _ => new ScriptNodeSaveData(),
        };
        serializer.Populate(obj.CreateReader(), nodeData);

        return nodeData;
    }

    public override void WriteJson(JsonWriter writer, ScriptNodeSaveData value, Newtonsoft.Json.JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}