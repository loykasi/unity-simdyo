using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Loykas.Scripting
{
    public class ScriptFlow : MonoBehaviour
    {
        public event UnityAction<ScriptNode> OnNodeAdded;
        public event UnityAction<ScriptNode> OnNodeDeleted;

        public event UnityAction<NodeConnection> OnConnectionDeleted;

        public event UnityAction<Variable> OnVariableAdded;
        public event UnityAction<Variable> OnVariableUpdated;
        public event UnityAction<Variable> OnVariableDeleted;

        public event UnityAction<ScriptFunction> OnFunctionDeleted;

        public Vector2 Pan { get; set; }
        public SceneEntity Entity;
        public bool IsGlobal => Entity == null;

        public List<ScriptNode> Nodes = new();
        public List<NodeConnection> Connections = new();
        public List<ScriptFunction> Functions = new();

        private Stack<int> _loops = new();

        private Dictionary<EventHook, List<EventNode>> _eventNodes = new();

        private List<NodeTask> _tasks = new();

        public Dictionary<string, Variable> Variables = new();
        public List<Variable> VariableList = new();
        private List<string> _variableOptions = new();


        public bool ShouldUpdateConnections { get; set; } = false;

        private List<string> _functionNames = new();    // For generate unique name
        private List<string> _variableNames = new();    // For generate unique name


        private void OnEnable()
        {
            if (SceneManager.Instance != null)
            {
                SceneManager.Instance.OnSceneStart += OnSceneStart;
                SceneManager.Instance.OnSceneStop += OnSceneStop;
            }
        }

        private void OnDisable()
        {
            if (SceneManager.Instance != null)
            {
                SceneManager.Instance.OnSceneStart -= OnSceneStart;
                SceneManager.Instance.OnSceneStop -= OnSceneStop;
            }
        }

        public void ResetState()
        {
            Nodes.Clear();
            Connections.Clear();
            Functions.Clear();
            Variables.Clear();

            _eventNodes.Clear();
        }

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

        public void AddNode(ScriptNode node)
        {
            AddNode(node, Vector3.zero);
        }

        public void AddNode(ScriptNode nodeContent, Vector3 position)
        {
            ScriptNode node = nodeContent.Create();

            node.Flow = this;
            node.Position = position;

            Nodes.Add(node);
            OnNodeAdded?.Invoke(node);

            if (node is EventNode eventNode)
            {
                eventNode.Register(this);
            }
        }

        public void AddNode(ScriptNode nodeContent, Vector3 position, IPort portToConnect, bool isSourcePort)
        {
            ScriptNode node = nodeContent.Create();

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
            foreach (IPort port in node.Ports())
            {
                Disconnect(port);
            }

            Nodes.Remove(node);

            if (node is FunctionEnterNode functionEnterNode)
            {
                DeleteFunction(functionEnterNode.Function);
            }

            OnNodeDeleted?.Invoke(node);
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
                    OnConnectionDeleted?.Invoke(connection);
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
                Trigger = outputTrigger.Invoke()
            };
            task.SetRemoveOnDone();
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

        public SceneEntity GetEntity(InputValue inputValue)
        {
            Debug.Log("Log from " + Entity.Id);
            int id = (int)inputValue.GetValue();
            SceneEntity entity = ObjectManager.Instance.GetEntityById(id);

            if (entity == null
                && inputValue.IsNullMeanSelf
                && !IsGlobal)
            {
                entity = Entity;
            }

            return entity;
        }

        public int GetCurrentLoop()
        {
            if (_loops.Count > 0)
            {
                return _loops.Peek();
            }

            return -1;
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

        private void OnSceneStart()
        {
            foreach (var item in Variables.Values)
            {
                item.OnSceneStart();
            }
        }

        private void OnSceneStop()
        {
            foreach (var item in Variables.Values)
            {
                item.OnSceneStop();
            }
        }

        // Add node from drag and drop

        public void AddGetVariableNode(Variable variable, Vector3 position)
        {
            ScriptNode node;
            if (IsGlobal)
            {
                GetGlobalVariableNode variableNode = ScriptNodeFactory.Instance.CreateNode<GetGlobalVariableNode>();
                variableNode.Flow = this;
                variableNode.Input.SetValue(variable.Name);
                node = variableNode;
            }
            else
            {
                GetVariableNode variableNode = ScriptNodeFactory.Instance.CreateNode<GetVariableNode>();
                variableNode.Flow = this;
                variableNode.Input.SetValue(variable.Name);
                node = variableNode;
            }
            node.Position = position;
            Nodes.Add(node);
            OnNodeAdded?.Invoke(node);
        }

        public void AddFunctionCallNode(ScriptFunction function, Vector3 position)
        {
            Debug.Log("Add function call");
            FunctionCallNode node = ScriptNodeFactory.Instance.CreateNode<FunctionCallNode>();

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

        private void DeleteFunction(ScriptFunction function)
        {
            Functions.Remove(function);
            OnFunctionDeleted?.Invoke(function);

            // remove all call node
            for (int i = Nodes.Count - 1; i >= 0; i--)
            {
                if (Nodes[i] is FunctionCallNode functionCallNode)
                {
                    if (functionCallNode.Function != function)
                    {
                        continue;
                    }

                    DeleteNode(functionCallNode);
                }
            }
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

        public Variable AddVariable()
        {
            string baseName = "NewVariable";
            string variableName = getVariableName(baseName);

            Variable variable = new()
            {
                Name = variableName
            };
            AddVariable(variable);

            OnVariableAdded?.Invoke(variable);
            return variable;
        }

        public void AddVariable(Variable variable)
        {
            Variables.Add(variable.Name, variable);
            VariableList.Add(variable);
        }

        private string getVariableName(string baseName)
        {
            _variableNames.Clear();
            _variableNames.AddRange(Variables.Keys);
            // for (int i = 0; i < Variables.Count; i++)
            // {
            //     _variableNames.Add(Variables[i].Name);
            // }

            return Utils.GenerateUniqueName(baseName, _variableNames);
        }

        public void UpdateVariable(string name, object value)
        {
            Variable variable = GetVariable(name);
            if (variable != null)
            {
                variable.Value = value;
                OnVariableUpdated?.Invoke(variable);
            }
        }

        public bool TryChangeVariableName(string oldName, string newName)
        {
            if (oldName == newName)
            {
                return false;
            }

            if (Variables.ContainsKey(newName))
            {
                ToastSystem.Instance.Show($"Variable named \"{newName}\" already exists");
                return false;
            }

            if (Variables.TryGetValue(oldName, out Variable variable))
            {
                Variables.Remove(oldName);
                Variables.Add(newName, variable);

                variable.SetName(newName);

                OnVariableUpdated?.Invoke(variable);
                return true;
            }

            return false;
        }

        public Variable GetVariable(string name)
        {
            if (Variables.TryGetValue(name, out Variable value))
            {
                return value;
            }
            return null;
        }

        public bool RemoveVariable(Variable variable)
        {
            if (Variables.Remove(variable.Name))
            {
                VariableList.Remove(variable);

                OnVariableDeleted?.Invoke(variable);
                return true;
            }

            return false;
        }

        public List<string> GetVariableOptions()
        {
            if (_variableOptions.Count == 0)
            {
                _variableOptions.Add("Select...");
                _variableOptions.AddRange(VariableList.Select(s => s.Name).ToList());
            }

            UpdateVaribleOptions();

            return _variableOptions;
        }

        private void UpdateVaribleOptions()
        {
            for (int i = 0; i < VariableList.Count; i++)
            {
                int index = i + 1;
                if (index < _variableOptions.Count)
                {
                    _variableOptions[i + 1] = VariableList[i].Name;
                }
                else
                {
                    _variableOptions.Add(VariableList[i].Name);
                }
            }

            int reserve = 1;
            int count = _variableOptions.Count - VariableList.Count - reserve;
            if (count > 0)
            {
                int from = _variableOptions.Count - count;
                _variableOptions.RemoveRange(from, count);
            }
        }
    }
}