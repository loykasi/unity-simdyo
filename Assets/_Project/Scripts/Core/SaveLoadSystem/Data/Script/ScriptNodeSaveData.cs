using System;
using System.Collections.Generic;
using UnityEngine;

public class ScriptNodeSaveData
{
    public string Type;
    public Guid ID;
    public Vector3 Position;
    public Dictionary<string, object> DefaultValues = new();
}