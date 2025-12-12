namespace Loykas.Scripting
{
    public static class ScriptFlowClone
    {
        public static void CloneScript(ScriptFlow a, ScriptFlow b)
        {
            foreach (var item in a.VariableList)
            {
                Variable variable = new
                (
                    item.Type,
                    item.Value
                );
                variable.Name = item.Name;
                b.AddVariable(variable);
            }

            foreach (var item in a.Functions)
            {
                ScriptFunction function = new()
                {
                    Name = item.Name
                };

                foreach (var input in item.Inputs)
                {
                    FunctionInput functionInput = new()
                    {
                        Name = input.Name,
                        Type = input.Type
                    };

                    function.Inputs.Add(functionInput);
                }

                b.Functions.Add(function);
            }

            foreach (var item in a.Nodes)
            {
                ScriptNode node = item.Create();
                node.Flow = b;
                node.ID = item.ID;
                node.Position = item.Position;

                if (node is FunctionEnterNode functionEnterNode)
                {
                    ScriptFunction function = b.Functions.Find(f => f.Name == functionEnterNode.Function.Name);

                    functionEnterNode.Init(function);
                    function.StartNode = functionEnterNode;
                }

                if (node is FunctionCallNode functionCallNode)
                {
                    ScriptFunction function = b.Functions.Find(f => f.Name == functionCallNode.Function.Name);

                    functionCallNode.Init(function);
                }
                
                foreach (var defaultValue in item.DefaultValues)
                {
                    node.DefaultValues[defaultValue.Key] = defaultValue.Value;
                }


                b.Nodes.Add(node);
            }

            foreach (var item in a.Connections)
            {
                NodeConnection connection = new()
                {
                    SourceID = item.SourceID,
                    SourceKey = item.SourceKey,
                    DestinationID = item.DestinationID,
                    DestinationKey = item.DestinationKey
                };

                b.Connections.Add(connection);
            }

            b.Load();
        }
    }
}