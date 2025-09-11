using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ScriptFlow : MonoBehaviour
{
    public event UnityAction<ScriptNode> OnNodeAdded;

    public SceneEntity Entity;

    public List<ScriptNode> Nodes = new();
    public List<NodeConnection> Connections = new();

    private int _loopIdentifier = 0;
    private Stack<int> _loops = new();

    private Dictionary<EventHook, List<EventNode>> _eventNodes = new();

    public Dictionary<string, Variable> Variables
    {
        get => _variables;
        set => _variables = value;
    }
    private Dictionary<string, Variable> _variables = new();

    public void Load()
    {
        foreach (var node in Nodes)
        {
            if (node is EventNode eventNode)
            {
                eventNode.Register(this);
            }
        }
        foreach (var connection in Connections)
        {
            connection.Load(this);
        }
    }

    public void AddNode(ScriptNodeData nodeData)
    {
        ScriptNode node = nodeData.Create();
        node.Flow = this;
        Nodes.Add(node);
        OnNodeAdded?.Invoke(node);

        if (node is EventNode eventNode)
        {
            eventNode.Register(this);
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
        TriggerEvent(EventHook.Start);
    }

    public void UpdateVS()
    {
        TriggerEvent(EventHook.Update);
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

    // Events

    public void RegisterEventNode(EventHook hook, EventNode node)
    {
        if (!_eventNodes.TryGetValue(hook, out var nodes))
        {
            nodes = new List<EventNode>();
            _eventNodes.Add(hook, nodes);
        }

        nodes.Add(node);
    }

    public void TriggerEvent(EventHook hook)
    {
        if (_eventNodes.TryGetValue(hook, out var nodes))
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                Invoke(nodes[i].Exit);
            }
        }
    }

    // Variables

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

    public void UpdateListVariable(string name, ListType subType)
    {
        if (_variables.TryGetValue(name, out Variable variable))
        {
            switch (subType)
            {
                case ListType.String:
                    variable.SubType = ListType.String;
                    variable.Value = new List<string>();
                    break;
                case ListType.Number:
                    variable.SubType = ListType.Number;
                    variable.Value = new List<float>();
                    break;
                case ListType.Boolean:
                    variable.SubType = ListType.Boolean;
                    variable.Value = new List<bool>();
                    break;
            }

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
            list[index] = value;
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

    public Variable GetVariable(string name)
    {
        if (_variables.TryGetValue(name, out Variable value))
        {
            return value;
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

    public List<string> GetVariableOptions()
    {
        return _variables.Select(s => s.Key).ToList();
    }
}