using System;
using System.Collections.Generic;
using System.Reflection;

namespace Loykas.Scripting
{
    public class ScriptNodeFactory : Singleton<ScriptNodeFactory>
    {
        //public List<ScriptNodeContent> Nodes = new();
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