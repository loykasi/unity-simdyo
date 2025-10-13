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

        private List<string> _functionNames = new();    // For generate new unique name

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

        public void AddNode(Type nodeType)
        {
            AddNode(nodeType, Vector3.zero);
        }

        public void AddNode(Type nodeType, Vector3 position)
        {
            ScriptNode node = ScriptNodeFactory.Instance.CreateNode(nodeType);

            node.Flow = this;
            node.Position = position;

            Nodes.Add(node);
            OnNodeAdded?.Invoke(node);

            if (node is EventNode eventNode)
            {
                eventNode.Register(this);
            }
        }

        public void AddNode(Type nodeType, Vector3 position, IPort portToConnect, bool isSourcePort)
        {
            ScriptNode node = ScriptNodeFactory.Instance.CreateNode(nodeType);

            node.Flow = this;
            node.Position = position;

            Nodes.Add(node);

            if (node is EventNode eventNode)
            {
                eventNode.Register(this);
            }

            foreach (IPort port in node.Ports())
            {
                if (port.CanConnect(portToConnect))
                {
                    if (isSourcePort)
                    {
                        TryConnect(portToConnect, port);
                    }
                    else
                    {
                        TryConnect(port, portToConnect);
                    }

                    break;
                }
            }
            
            OnNodeAdded?.Invoke(node);
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

        public void Disconnect(IPort port)
        {
            for (int i = 0; i < Connections.Count; i++)
            {
                NodeConnection connection = Connections[i];
                if (connection.Source == port || connection.Destination == port)
                {
                    connection.Source.Disconnect(connection.Destination);
                    connection.Destination.Disconnect(connection.Source);

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

        public IEnumerable<NodeConnection> GetConnections(IScriptNode node)
        {
            return Connections.Where(c => c.Source.Node == node || c.Destination.Node == node);
        }

        // handle node task

        public NodeTask GetNodeTask(OutputTrigger from)
        {
            return _tasks.Find(t => t.From == from);
        }

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
            task.SetRemoveOnDone(true);
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
            GetVariableNode node = ScriptNodeFactory.Instance.CreateNode<GetVariableNode>();

            node.Input.SetValue(key);

            node.Flow = this;
            Nodes.Add(node);
            OnNodeAdded?.Invoke(node);
        }

        public void AddFunctionCallNode(ScriptFunction function, Vector3 position)
        {
            Debug.Log("Add function call");
            FunctionCallNode node = ScriptNodeFactory.Instance.CreateNode<FunctionCallNode>();

            function.CallNode = node;
            node.Init(function);
            node.Position = position;

            node.Flow = this;
            Nodes.Add(node);
            OnNodeAdded?.Invoke(node);
        }


        // Function

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
            AddEnterFuncionNode(function);

            return function;
        }

        private void AddEnterFuncionNode(ScriptFunction function)
        {
            FunctionEnterNode node = ScriptNodeFactory.Instance.CreateNode<FunctionEnterNode>();

            function.StartNode = node;
            node.Init(function);

            node.Flow = this;
            Nodes.Add(node);
            OnNodeAdded?.Invoke(node);
        }

        private string GetFunctionName(string baseName)
        {
            _functionNames.Clear();
            for (int i = 0; i < Functions.Count; i++)
            {
                _functionNames.Add(Functions[i].Name);
            }

            return Utils.GenerateUniqueName(baseName, _functionNames);
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