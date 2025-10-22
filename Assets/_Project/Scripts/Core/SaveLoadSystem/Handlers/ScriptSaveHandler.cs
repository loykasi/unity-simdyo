using System;
using Loykas.Scripting;
using UnityEngine;

public static class ScriptSaveHandler
{
    public static void Save(ScriptFlowData flowData, ScriptFlow flow)
    {
        flowData.Pan = flow.Pan;

        for (int i = 0; i < flow.Nodes.Count; i++)
        {
            ScriptNode node = flow.Nodes[i];
            string typeName = node.GetType().Name;

            ScriptNodeSaveData nodeData;

            if (node is FunctionEnterNode functionEnterNode)
            {
                nodeData = new ScriptNodeFunctionSaveData()
                {
                    Name = functionEnterNode.Function.Name
                };
            }
            else if (node is FunctionCallNode functionCallNode)
            {
                nodeData = new ScriptNodeFunctionSaveData()
                {
                    Name = functionCallNode.Function.Name
                };
            }
            else
            {
                nodeData = new();
            }

            nodeData.Type = typeName.Substring(0, typeName.Length - 4);
            nodeData.ID = node.ID;
            nodeData.Position = node.Position;

            foreach (var item in node.DefaultValues)
            {
                nodeData.DefaultValues.Add(item.Key, item.Value);
            }

            flowData.Nodes.Add(nodeData);
        }

        for (int i = 0; i < flow.Connections.Count; i++)
        {
            NodeConnection connection = flow.Connections[i];

            ScriptConnectionSaveData connectionData = new()
            {
                SourceID = connection.SourceID,
                SourceKey = connection.SourceKey,
                DestinationID = connection.DestinationID,
                DestinationKey = connection.DestinationKey,
            };

            flowData.Connections.Add(connectionData);
        }

        foreach (Variable variable in flow.Variables.Values)
        {   
            ScriptVariableSaveData variableData = new()
            {
                Type = variable.Type.Type,
                IsList = variable.Type.IsList,
                Value = variable.Value
            };
            flowData.Variables.Add(variableData);
        }

        for (int i = 0; i < flow.Functions.Count; i++)
        {
            ScriptFunction function = flow.Functions[i];

            ScriptFunctionSaveData saveData = new()
            {
                Name = function.Name,
            };

            for (int j = 0; j < function.Inputs.Count; j++)
            {
                FunctionInput input = function.Inputs[j];
                ScriptFunctionInputSaveData inputData = new()
                {
                    Name = input.Name,
                    Type = input.Type.Type,
                    IsList = input.Type.IsList
                };
                saveData.Inputs.Add(inputData);
            }

            flowData.Functions.Add(saveData);
        }
    }

    public static void Load(ScriptFlowData flowData, ScriptFlow flow)
    {
        flow.Pan = flowData.Pan;

        flow.Functions.Clear();
        for (int i = 0; i < flowData.Functions.Count; i++)
        {
            ScriptFunctionSaveData saveData = flowData.Functions[i];

            ScriptFunction function = new();
            function.Name = saveData.Name;

            for (int j = 0; j < saveData.Inputs.Count; j++)
            {
                ScriptFunctionInputSaveData inputData = saveData.Inputs[i];
                FunctionInput functionInput = new()
                {
                    Name = inputData.Name,
                    Type = new ScriptDataType(inputData.Type, inputData.IsList)
                };

                function.Inputs.Add(functionInput);
            }

            flow.Functions.Add(function);
        }

        flow.Nodes.Clear();
        for (int i = 0; i < flowData.Nodes.Count; i++)
        {
            ScriptNodeSaveData saveData = flowData.Nodes[i];

            string typeName = "Loykas.Scripting." + saveData.Type + "Node";
            Type type = Type.GetType(typeName);

            ScriptNode node = ScriptNodeFactory.Instance.CreateNode(type);
            node.Flow = flow;
            node.ID = saveData.ID;
            node.Position = saveData.Position;

            if (saveData is ScriptNodeFunctionSaveData nodeFunctionData)
            {
                string funcionName = nodeFunctionData.Name;
                Debug.Log(funcionName);

                ScriptFunction function = flow.Functions.Find(f => f.Name == funcionName);

                if (node is FunctionEnterNode functionEnterNode)
                {
                    functionEnterNode.Init(function);
                    function.StartNode = functionEnterNode;
                }
                if (node is FunctionCallNode functionCallNode)
                {
                    functionCallNode.Init(function);
                }
            }
            
            foreach (var item in saveData.DefaultValues)
            {
                Debug.Log($"Load value: {item.Value}");
                object value;
                if (item.Value.GetType() == typeof(double))
                {
                    value = (float)(double)item.Value;
                }
                else
                {
                    value = item.Value;
                }
                
                if (!node.DefaultValues.ContainsKey(item.Key))
                {
                    node.DefaultValues.Add(item.Key, value);
                    continue;
                }
                node.DefaultValues[item.Key] = value;
            }


            flow.Nodes.Add(node);
        }

        flow.Connections.Clear();
        for (int i = 0; i < flowData.Connections.Count; i++)
        {
            ScriptConnectionSaveData saveData = flowData.Connections[i];

            NodeConnection connection = new();
            connection.SourceID = saveData.SourceID;
            connection.SourceKey = saveData.SourceKey;
            connection.DestinationID = saveData.DestinationID;
            connection.DestinationKey = saveData.DestinationKey;

            flow.Connections.Add(connection);
        }

        flow.Variables.Clear();
        foreach (var item in flowData.Variables)
        {
            Variable variable = new
            (
                new ScriptDataType(item.Type, item.IsList),
                item.Value
            );
            
            flow.Variables.Add(variable.Name, variable);
        }

        flow.Load();
    }
}