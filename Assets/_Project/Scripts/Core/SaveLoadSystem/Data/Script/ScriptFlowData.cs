using System.Collections.Generic;
using Loykas.Scripting;
using UnityEngine;

public class ScriptFlowData
{
    public Vector2 Pan;
    public List<ScriptNodeSaveData> Nodes = new();
    public List<ScriptConnectionSaveData> Connections = new();
    public List<ScriptVariableSaveData> Variables = new();
    public List<ScriptFunctionSaveData> Functions = new();
    
    public void Clear()
    {
        Nodes.Clear();
        Connections.Clear();
        Variables.Clear();
        Functions.Clear();
    }
}