using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Loykas.Scripting
{
    public class ScriptNodeFactory : Singleton<ScriptNodeFactory>
    {
        public Dictionary<Type, ScriptNodeContent> Nodes = new();

        protected override void Awake()
        {
            base.Awake();

            LoadNodes();
        }

        private void LoadNodes()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            Type[] types = assembly.GetTypes();

            foreach (Type type in types)
            {
                ScriptNodeAttribute attribute = type.GetCustomAttribute<ScriptNodeAttribute>();
                if (attribute != null)
                {
                    var content = (ScriptNodeContent)Activator.CreateInstance(type);
                    content.Category = attribute.Category;
                    Nodes.Add(content.Type, content);
                }
            }
        }

        public void GetNodes(List<ScriptNodeContent> nodes)
        {
            nodes.Clear();
            nodes.AddRange(Nodes.Values);
        }

        public void GetNodes(List<ScriptNodeContent> nodes, IPort port)
        {
            nodes.Clear();
            foreach (ScriptNodeContent node in Nodes.Values)
            {
                if (port is InputValue)
                {
                    for (int i = 0; i < node.Base.ValueOutputs.Count; i++)
                    {
                        var output = node.Base.ValueOutputs[i];
                        if (port.CanConnect(output))
                        {
                            nodes.Add(node);
                        }
                    }
                }
                if (port is OutputValue)
                {
                    for (int i = 0; i < node.Base.ValueInputs.Count; i++)
                    {
                        var input = node.Base.ValueInputs[i];
                        if (port.CanConnect(input))
                        {
                            nodes.Add(node);
                        }
                    }
                }
                if (port is InputTrigger)
                {
                    if (node.Base.OutputTriggers.Count > 0)
                    {
                        nodes.Add(node);
                    }
                }
                if (port is OutputTrigger)
                {
                    if (node.Base.InputTriggers.Count > 0)
                    {
                        nodes.Add(node);
                    }
                }
            }
        }

        public T CreateNode<T>() where T : ScriptNode
        {
            if (Nodes.TryGetValue(typeof(T), out ScriptNodeContent content))
            {
                ScriptNode node = content.Create();
                return (T)node;
            }
            return null;
        }

        public ScriptNode CreateNode(Type nodeType)
        {
            if (Nodes.TryGetValue(nodeType, out ScriptNodeContent content))
            {
                ScriptNode node = content.Create();
                return node;
            }
            return null;
        }
    }
}