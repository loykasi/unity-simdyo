using System;
using Newtonsoft.Json;
using UnityEngine;

public class NodeConnection
{
    public Guid SourceID;
    public string SourceKey;

    public Guid DestinationID;
    public string DestinationKey;

    [JsonIgnore]
    public IPort Source;
    [JsonIgnore]
    public IPort Destination;

    public NodeConnection()
    {
    }

    public NodeConnection(IPort source, IPort destination)
    {
        Source = source;
        Destination = destination;

        SourceID = Source.Node.ID;
        SourceKey = Source.Key;
        DestinationID = Destination.Node.ID;
        DestinationKey = Destination.Key;
    }

    public void Load(VisualScripting vs)
    {
        Source = GetPort(vs, SourceID, SourceKey);
        Destination = GetPort(vs, DestinationID, DestinationKey);

        if (Source is OutputTrigger outputTrigger && Destination is InputTrigger inputTrigger)
        {
            outputTrigger.Destination = inputTrigger;
            inputTrigger.Sources.Add(outputTrigger);
        }

        if (Source is OutputValue outputValue && Destination is InputValue inputValue)
        {
            inputValue.Source = outputValue;
            outputValue.Destinations.Add(inputValue);
        }
    }

    private IPort GetPort(VisualScripting vs, Guid id, string portKey)
    {
        ScriptNode unit = vs.Nodes.Find(node => node.ID == id);
        foreach (var item in unit.Ports())
        {
            if (item.Key.Equals(portKey))
            {
                return item;
            }
        }
        return null;
    }
}