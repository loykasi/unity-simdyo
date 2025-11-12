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
                    content.ShouldIncludeInMenu = attribute.ShoudlIncludeInMenu;
                    Nodes.Add(content.Type, content);
                }
            }
        }

        public void GetNodes(List<ScriptNodeContent> nodes, bool isGlobal = false)
        {
            nodes.Clear();
            foreach (ScriptNodeContent node in Nodes.Values)
            {
                if (node.ShouldIncludeInMenu)
                {
                    if (isGlobal && !node.CanUseGlobal)
                    {
                        continue;
                    }

                    if (nodes.Count == 0)
                    {
                        nodes.Add(node);
                        continue;
                    }

                    SortedAdd(nodes, node);
                }
            }
        }

        public void GetNodes(List<ScriptNodeContent> nodes, IPort port, bool isGlobal = false)
        {
            nodes.Clear();
            foreach (ScriptNodeContent node in Nodes.Values)
            {
                if (!node.ShouldIncludeInMenu)
                {
                    continue;
                }

                if (isGlobal && !node.CanUseGlobal)
                {
                    continue;
                }

                if (port is InputValue)
                {
                    for (int i = 0; i < node.Base.ValueOutputs.Count; i++)
                    {
                        var output = node.Base.ValueOutputs[i];
                        if (port.CanConnect(output))
                        {
                            SortedAdd(nodes, node);
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
                            SortedAdd(nodes, node);
                        }
                    }
                }
                if (port is InputTrigger)
                {
                    if (node.Base.OutputTriggers.Count > 0)
                    {
                        SortedAdd(nodes, node);
                    }
                }
                if (port is OutputTrigger)
                {
                    if (node.Base.InputTriggers.Count > 0)
                    {
                        SortedAdd(nodes, node);
                    }
                }
            }
        }

        private void SortedAdd(List<ScriptNodeContent> nodes, ScriptNodeContent node)
        {
            int categoryOrder = (int)node.Category;
            for (int i = 0; i < nodes.Count; i++)
            {
                int currentCategoryOrder = (int)nodes[i].Category;
                if (categoryOrder < currentCategoryOrder)
                {
                    nodes.Insert(i, node);
                    return;
                }
            }
            
            nodes.Add(node);
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