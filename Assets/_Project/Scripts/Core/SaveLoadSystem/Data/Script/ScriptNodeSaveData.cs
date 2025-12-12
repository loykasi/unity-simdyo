using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class ScriptNodeSaveData
{
    public string Type;
    public Guid ID;
    public Vector3 Position;
    // [JsonConverter()]
    // public Dictionary<string, object> DefaultValues = new();
    public List<ScriptNodeValueData> DefaultValues = new();

}