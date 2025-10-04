using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;

namespace Loykas.Scripting
{
    public class ScriptFlow : MonoBehaviour
    {
        public event UnityAction<ScriptNode> OnNodeAdded;
        public event UnityAction OnVariableAdded;

        public Vector2 Pan { get; set; }
        public SceneEntity Entity;

        [SerializeField] private GetVariable _getVariableNodeData;

        public List<ScriptNode> Nodes = new();
        public List<NodeConnection> Connections = new();
        public List<ScriptFunction> Functions = new();

        private int _loopIdentifier = 0;
        private Stack<int> _loops = new();

        private Dictionary<EventHook, List<EventNode>> _eventNodes = new();

        private List<NodeTask> _tasks = new();

        public Dictionary<string, Variable> Variables
        {
            get => _variables;
            set => _variables = value;
        }
        private Dictionary<string, Variable> _variables = new();

        public bool ShouldUpdateConnections { get; set; } = false;

        private void LateUpdate()
        {
            if (ShouldUpdateConnections)
            {
                Connections.RemoveAll(c => c.ShouldRemove);
                ShouldUpdateConnections = false;
            }
        }

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

        public void DeleteNode(ScriptNode node)
        {
            Nodes.Remove(node);
        }

        public bool TryConnect(IPort fromPort, IPort toPort)
        {
            if (fromPort.ConnectToPort(toPort) && toPort.ConnectToPort(fromPort))
            {
                Connections.Add(new NodeConnection(this, fromPort, toPort));
                return true;
            }

            return false;
        }

        public void Disconnect(IPort source, IPort destination)
        {
            for (int i = 0; i < Connections.Count; i++)
            {
                NodeConnection connection = Connections[i];
                if (connection.Source == source && connection.Destination == destination)
                {
                    source.Disconnect(destination);
                    destination.Disconnect(source);


                    ShouldUpdateConnections = true;
                    connection.ShouldRemove = true;
                }
            }
        }

        public IEnumerable<NodeConnection> GetConnections(IPort port)
        {
            return Connections.Where(c => c.Source == port || c.Destination == port);
        }

        public NodeConnection GetConnection(IPort source, IPort destination)
        {
            return Connections.Find(c => c.Source == source && c.Destination == destination);
        }


        // handle node task

        public NodeTask GetNodeTask(InputTrigger trigger)
        {
            return _tasks.Find(t => t.Trigger == trigger);
        }

        public void Invoke(OutputTrigger outputTrigger)
        {
            bool exist = _tasks.Find(t => t.From == outputTrigger) != null;
            if (exist)
            {
                return;
            }

            NodeTask task = new()
            {
                From = outputTrigger,
                Trigger = outputTrigger.Invoke(this)
            };
            _tasks.Add(task);
        }

        public void UpdateTask()
        {
            for (int i = 0; i < _tasks.Count; i++)
            {
                _tasks[i].Invoke(this);
            }
        }

        public void RemoveTask(NodeTask task)
        {
            _tasks.Remove(task);
        }

        //

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
            UpdateTask();
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

        // Add node from drag and drop

        public void AddGetVariableNode(string key)
        {
            GetVariableNode node = (GetVariableNode)_getVariableNodeData.Create();

            node.Input.SetValue(key);

            node.Flow = this;
            Nodes.Add(node);
            OnNodeAdded?.Invoke(node);
        }

        public ScriptFunction AddFunction()
        {
            string baseName = "NewFunction";
            string functionName = GetFunctionName(baseName);

            Debug.Log($"Function: {functionName}");

            ScriptFunction function = new()
            {
                Name = functionName
            };

            Functions.Add(function);

            return function;
        }

        private string GetFunctionName(string baseName)
        {
            string pattern = @$"^{baseName}(?: \((\d+)\))?$";

            Regex regex = new(pattern, RegexOptions.Compiled);

            List<int> ints = new();

            int i = 0;

            for (i = 0; i < Functions.Count; i++)
            {
                Match match = regex.Match(Functions[i].Name);
                if (match.Success)
                {
                    string value = match.Groups[1].Value;
                    int number = value == string.Empty ? 0 : int.Parse(value);
                    ints.Add(number);
                }
            }
            ints.Sort();

            for (i = 0; i < ints.Count; i++)
            {
                if (i != ints[i])
                {
                    break;
                }
            }

            // Debug.Log(string.Join(" ", ints));

            if (i == 0)
            {
                return baseName;
            }
            return string.Concat(baseName, " (", i, ")");
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
                    var node = nodes[i];
                    if (node.ShouldTrigger())
                    {
                        Invoke(node.Exit);
                    }
                }
            }
        }

        public void TriggerEvent(EventHook hook, object args)
        {
            if (_eventNodes.TryGetValue(hook, out var nodes))
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    var node = nodes[i];
                    if (node.ShouldTrigger())
                    {
                        node.AssignArgument(args);
                        Invoke(node.Exit);
                    }
                }
            }
        }

        public void SendSignal(string signalName)
        {
            if (_eventNodes.TryGetValue(EventHook.Signal, out var nodes))
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    var node = (OnReceiveSignalNode)nodes[i];
                    if (node.DefaultValues["Name"].Equals(signalName))
                    {
                        Invoke(node.Exit);
                    }
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
                _variables.Add(name, new Variable());
                return true;
            }

            return false;
        }

        public void UpdateVariable(string name, object value)
        {
            if (_variables.TryGetValue(name, out Variable variable))
            {
                Debug.Log($"Update {name} = {value}");
                variable.Value = value;
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
}