using System.Collections.Generic;

public class ScriptElementData
{
    public List<ScriptNode> Nodes = new();
    public List<NodeConnection> Connections = new();
    public Dictionary<string, Variable> Variables = new();
}