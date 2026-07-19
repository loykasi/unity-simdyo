using System;
using Loykas.Scripting;
using UnityEngine;

public static class ScriptSaveHandler
{
    public static void Save(ScriptFlowData flowData, ScriptFlow flow)
    {
        flowData.Pan = flow.Pan;
        flowData.Scale = flow.Scale;

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

            // Remove string "Node"
            nodeData.Type = typeName.Substring(0, typeName.Length - 4);

            nodeData.ID = node.ID;
            nodeData.Position = node.Position;

            foreach (var (key, value) in node.DefaultValues)
            {
                InputValue input = node.ValueInputs.Find(n => n.Key == key);

                if (value.GetObjectValue() is Key)
                {
                    nodeData.DefaultValues.Add(new ScriptNodeValueData(key, value.GetObjectValue().ToString(), input.Type.Type));
                }
                else
                {
                    Debug.Log(value.GetObjectValue());
                    nodeData.DefaultValues.Add(new ScriptNodeValueData(key, value.GetObjectValue(), input.Type.Type));
                }
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
                Name = variable.Name,
                Type = variable.Type.Type,
                Kind = variable.Type.Kind,
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
                    Kind = input.Type.Kind
                };
                saveData.Inputs.Add(inputData);
            }

            flowData.Functions.Add(saveData);
        }
    }

    public static void Load(ScriptFlowData flowData, ScriptFlow flow)
    {
        flow.Pan = flowData.Pan;
        flow.Scale = flowData.Scale;

        flow.Variables.Clear();
        foreach (var item in flowData.Variables)
        {
            Variable variable = new
            (
                new ScriptDataType(item.Type, item.Kind),
                item.Value
            );
            variable.Name = item.Name;
            
            flow.AddVariable(variable);
        }

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
                    Type = new ScriptDataType(inputData.Type, inputData.Kind)
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
                if (!node.DefaultValues.ContainsKey(item.Key))
                {
                    node.DefaultValues.Add(item.Key, ValueTransfer.FromValue(item.Type, item.Value));
                    continue;
                }
                node.DefaultValues[item.Key] = ValueTransfer.FromValue(item.Type, item.Value);
            }

            flow.Nodes.Add(node);
            node.Init();
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
            connection.Flow = flow;

            flow.Connections.Add(connection);
        }

        flow.Load();
    }
}