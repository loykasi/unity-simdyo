using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class VisualScripting : MonoBehaviour
{
    public event UnityAction<ScriptNode> OnNodeAdded;

    public ScriptNodeData startNodeData;
    public EventNode startNode;

    private void Awake()
    {
        startNode = startNodeData.Create() as EventNode;
    }

    public void AddNode(ScriptNodeData nodeData)
    {
        ScriptNode node = nodeData.Create();
        OnNodeAdded?.Invoke(node);
    }

    public bool TryConnect(ScriptNode fromNode, IPort fromPort, ScriptNode toNode, IPort toPort)
    {
        if (fromPort.ConnectToPort(toPort) && toPort.ConnectToPort(fromPort))
        {
            Debug.Log($"Connect successful");
            return true;
        }

        Debug.Log($"Connect failed");
        return false;
    }

    public void Invoke(OutputTrigger outputTrigger)
    {
        // InputTrigger input = outputTrigger.Destination;
        // OutputTrigger output = input.Action();
        // Invoke(output);
        outputTrigger.Invoke();
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(1810, 10, 100, 50), "Run"))
        {
            Invoke(startNode.outputTrigger);
        }
    }
}