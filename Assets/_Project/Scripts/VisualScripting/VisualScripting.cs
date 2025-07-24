using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VisualScripting : MonoBehaviour
{
    public event UnityAction<ScriptNode> OnNodeAdded;

    public List<ScriptNode> Nodes = new();
    public List<NodeConnection> Connections = new();

    private int _loopIdentifier = 0;
    private Stack<int> _loops = new();

        private List<EventNode> _startNodes = new();
    private List<EventNode> _updateNodes = new();

    public void AddNode(ScriptNodeData nodeData)
    {
        ScriptNode node = nodeData.Create();
        Nodes.Add(node);
        OnNodeAdded?.Invoke(node);

        if (node is EventNode eventNode)
        {
            switch (eventNode.GetHook())
            {
                case EventHook.Start:
                    _startNodes.Add(eventNode);
                    break;
                case EventHook.Update:
                    _updateNodes.Add(eventNode);
                    break;
            }
        }
    }

    public bool TryConnect(ScriptNode fromNode, IPort fromPort, ScriptNode toNode, IPort toPort)
    {
        if (fromPort.ConnectToPort(toPort) && toPort.ConnectToPort(fromPort))
        {
            Connections.Add(new NodeConnection(fromPort, toPort));
            Debug.Log($"Connect successful");
            return true;
        }

        Debug.Log($"Connect failed");
        return false;
    }

    public void Invoke(OutputTrigger outputTrigger)
    {
        outputTrigger.Invoke(this);
    }

    public int GetCurrentLoop()
    {
        if (_loops.Count > 0)
        {
            return _loops.Peek();
        }

        return -1;
    }

    public bool IsLoopNotBroken(int loop)
    {
        return GetCurrentLoop() == loop;
    }

    public int StartLoop()
    {
        int loop = _loopIdentifier++;
        _loops.Push(loop);

        return loop;
    }

    public void BreakLoop()
    {
        if (GetCurrentLoop() < 0)
        {
            return;
        }

        _loopIdentifier--;
        _loops.Pop();
    }

    public void ExitLoop(int loop)
    {
        if (loop != GetCurrentLoop())
        {
            return;
        }

        _loopIdentifier--;
        _loops.Pop();
    }

    public void StartVS()
    {
        foreach (var item in _startNodes)
        {
            Invoke(item.outputTrigger);
        }
    }

    public void UpdateVS()
    {
        for (int i = 0; i < _updateNodes.Count; i++)
        {
            Invoke(_updateNodes[i].outputTrigger);
        }
    }
}