using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Loykas.Scripting
{
    public class ScriptNodeFactory : Singleton<ScriptNodeFactory>
    {
        public Dictionary<Type, ScriptNode> Nodes;
        private ScriptNode[] _nodes;

        protected override void Awake()
        {
            base.Awake();

            LoadNodes();
        }

        private void LoadNodes()
        {
            // Assembly assembly = Assembly.GetExecutingAssembly();

            // Type[] types = assembly.GetTypes();

            // foreach (Type type in types)
            // {
            //     ScriptNodeAttribute attribute = type.GetCustomAttribute<ScriptNodeAttribute>();
            //     if (attribute != null)
            //     {
            //         var content = (ScriptNodeContent)Activator.CreateInstance(type);
            //         content.Category = attribute.Category;
            //         content.ShouldIncludeInMenu = attribute.ShoudlIncludeInMenu;
            //         Nodes.Add(content.Type, content);
            //     }
            // }

            _nodes = new ScriptNode[]
            {
                // Event
                new StartNode(),
                new UpdateNode(),
                new SendSignalNode(),
                new OnReceiveSignalNode(),
                new OnClickedNode(),
                new OnKeyPressedNode(),
                new OnTouchedNode(),
                new StartAsCloneNode(),
                
                // Control
                new WaitNode(),
                new BranchNode(),
                new ForNode(),
                new BreakNode(),
                new CreateCloneNode(),
                new DeleteSelfNode(),
                // new RestartNode(),
                // new PauseNode(),
                // new ResumeNode(),

                // Motion,
                new SetPositionNode(),
                new SetAngleNode(),
                new SetColliderNode(),
                new SetGravityNode(),
                new SetVelocityNode(),
                new SetCollisionLayerNode(),
                
                new GetPositionNode(),
                new GetAngleNode(),
                new GetColliderNode(),
                new GetGravityNode(),
                new GetVelocityNode(),

                // Look,
                new SetColorNode(),
                new SetTextureSlotNode(),
                new SetSizeNode(),
                new SetRadiusNode(),
                new SetTextNode(),

                new GetColorNode(),
                new GetTextureSlotNode(),
                new GetSizeNode(),
                new GetRadiusNode(),
                new GetTextNode(),

                // Operator,
                new AddNode(),
                new SubtractNode(),
                new MultiplyNode(),
                new DivideNode(),
                new ModuloNode(),

                new JoinNode(),

                new EqualNode(),
                new GreaterNode(),
                new GreaterEqualNode(),
                new LessNode(),
                new LessEqualNode(),
                new NotNode(),

                new RandomNumberNode(),
                new RandomBooleanNode(),

                new ConvertToStringNode(),
                new ConvertToNumberNode(),

                // Data,
                new SetVariableNode(),
                new GetVariableNode(),
                new SetGlobalVariableNode(),
                new GetGlobalVariableNode(),
                new MakeStringNode(),
                new MakeNumberNode(),
                new MakeBooleanNode(),
                new MakeColorNode(),

                // List
                new AddListItemNode(),
                new RemoveListItemNode(),
                new InsertListItemNode(),
                new SetListItemNode(),
                new ClearListNode(),
                new GetListItemNode(),
                new GetListLengthNode(),
                new ListContainsItemNode(),

                // Debug
                new LogNode(),

                // Camera
                new SetCameraSizeNode(),
                new SetCameraPositionNode(),
                new GetCameraSizeNode(),
                new GetCameraPositionNode(),

                // Hidden
                new FunctionCallNode(),
                new FunctionEnterNode()
            };

            Nodes = new(_nodes.Length);
            foreach (ScriptNode node in _nodes)
            {
                Nodes.Add(node.GetType(), node);
            }
        }

        public void GetNodes(List<ScriptNode> nodes, bool isGlobal = false)
        {
            nodes.Clear();
            foreach (ScriptNode node in _nodes)
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

        public void GetNodes(List<ScriptNode> nodes, IPort port, bool isGlobal = false)
        {
            nodes.Clear();
            foreach (ScriptNode node in _nodes)
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
                    for (int i = 0; i < node.ValueOutputs.Count; i++)
                    {
                        var output = node.ValueOutputs[i];
                        if (port.CanConnect(output))
                        {
                            SortedAdd(nodes, node);
                            break;
                        }
                    }
                }
                if (port is OutputValue)
                {
                    for (int i = 0; i < node.ValueInputs.Count; i++)
                    {
                        var input = node.ValueInputs[i];
                        if (port.CanConnect(input))
                        {
                            SortedAdd(nodes, node);
                            break;
                        }
                    }
                }
                if (port is InputTrigger)
                {
                    if (node.OutputTriggers.Count > 0)
                    {
                        SortedAdd(nodes, node);
                    }
                }
                if (port is OutputTrigger)
                {
                    if (node.InputTriggers.Count > 0)
                    {
                        SortedAdd(nodes, node);
                    }
                }
            }
        }

        private void SortedAdd(List<ScriptNode> nodes, ScriptNode node)
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
            if (Nodes.TryGetValue(typeof(T), out ScriptNode node))
            {
                return (T)node.Create();
            }
            return null;
        }

        public ScriptNode CreateNode(Type nodeType)
        {
            if (Nodes.TryGetValue(nodeType, out ScriptNode node))
            {
                return node.Create();
            }
            return null;
        }
    }
}