using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VisualScripting : MonoBehaviour
{
    public event UnityAction<ScriptNode> OnNodeAdded;

    public SceneEntity Entity;

    public List<ScriptNode> Nodes = new();
    public List<NodeConnection> Connections = new();

    private int _loopIdentifier = 0;
    private Stack<int> _loops = new();

    private List<EventNode> _startNodes = new();
    private List<EventNode> _updateNodes = new();

    public Dictionary<string, Variable> Variables => _variables;
    private Dictionary<string, Variable> _variables = new();

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

    public void OnSceneStart()
    {
        foreach (var item in _variables)
        {
            item.Value.OnSceneStart();
        }
    }

    public void OnSceneStop()
    {
        foreach (var item in _variables)
        {
            item.Value.OnSceneStop();
        }
    }

    public bool AddVariable(string name)
    {
        if (name.Equals(string.Empty))
        {
            Debug.Log("Variable cannot be empty");
            return false;
        }

        if (!_variables.ContainsKey(name))
        {
            _variables.Add(name, new Variable(DataType.String, ""));
            Debug.Log("Add variable");
            return true;
        }

        return false;
    }

    public void UpdateVariable(string name, DataType type, object value)
    {
        if (_variables.TryGetValue(name, out Variable variable))
        {
            Debug.Log($"Update variable {name} =  {value}");
            variable.Type = type;
            variable.Value = value;
        }
    }

    public void UpdateVariable(string name, object value)
    {
        if (_variables.TryGetValue(name, out Variable variable))
        {
            Debug.Log($"Update {name} = {value}");
            variable.Value = value;
        }
    }

    public void UpdateListVariable(string name)
    {
        if (_variables.TryGetValue(name, out Variable variable))
        {
            Debug.Log("UpdateListVariable");
            variable.Value = new List<string>();
            variable.Type = DataType.List;
        }
    }

    public void InsertListItem(string name, object value)
    {
        if (_variables.TryGetValue(name, out Variable variable))
        {
            IList list = (IList)variable.Value;
            list.Add(value);
        }
    }

    public void UpdateListItem(string name, int index, object value)
    {
        if (_variables.TryGetValue(name, out Variable variable))
        {
            IList list = (IList)variable.Value;
            list[index] = (string)value;
        }
    }

    public void RemoveListItem(string name, int index)
    {
        if (_variables.TryGetValue(name, out Variable variable))
        {
            IList list = (IList)variable.Value;
            list.RemoveAt(index);
        }
    }

    public object GetVariable(string name)
    {
        if (_variables.TryGetValue(name, out Variable value))
        {
            return value.Value;
        }
        return null;
    }

    public bool RemoveVariable(string name)
    {
        if (_variables.ContainsKey(name))
        {
            _variables.Remove(name);
            return true;
        }
        return false;
    }
}