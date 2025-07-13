using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class VisualScripting : MonoBehaviour
{
    public event UnityAction<ScriptNode> OnNodeAdded;

    public ScriptNodeData startNodeData;
    public EventNode startNode;

    private int _loopIdentifier = 0;
    private Stack<int> _loops = new Stack<int>();

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

    private void OnGUI()
    {
        if (GUI.Button(new Rect(1810, 10, 100, 50), "Run"))
        {
            Invoke(startNode.outputTrigger);
        }
    }
}